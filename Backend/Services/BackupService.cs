using Backend.Interfaces.Services;
using MySqlConnector;
using System.Text;

namespace Backend.Services
{
    public class BackupService : IBackupService
    {
        private readonly string _connectionString;

        public BackupService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? throw new ArgumentException("Connection string not found");
        }

        public async Task<byte[]> GenerateBackupAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            // Obtener el nombre de la base de datos de la cadena de conexión
            var builder = new MySqlConnectionStringBuilder(_connectionString);
            var databaseName = builder.Database;
            
            var backupContent = new StringBuilder();
            
            // Header del backup
            backupContent.AppendLine("-- MySQL dump");
            backupContent.AppendLine($"-- Host: {builder.Server}    Database: {databaseName}");
            backupContent.AppendLine($"-- Generated at: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            backupContent.AppendLine("-- Server version: MySQL 8.0");
            backupContent.AppendLine();
            
            // Configuraciones iniciales
            backupContent.AppendLine("/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;");
            backupContent.AppendLine("/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;");
            backupContent.AppendLine("/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;");
            backupContent.AppendLine("/*!50503 SET NAMES utf8mb4 */;");
            backupContent.AppendLine("/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;");
            backupContent.AppendLine("/*!40103 SET TIME_ZONE='+00:00' */;");
            backupContent.AppendLine("/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;");
            backupContent.AppendLine("/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;");
            backupContent.AppendLine("/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;");
            backupContent.AppendLine("/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;");
            backupContent.AppendLine();
            
            // Crear base de datos
            backupContent.AppendLine($"-- Create database if not exists");
            backupContent.AppendLine($"CREATE DATABASE IF NOT EXISTS `{databaseName}` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;");
            backupContent.AppendLine($"USE `{databaseName}`;");
            backupContent.AppendLine();
            
            // Obtener todas las tablas ordenadas por dependencias
            var tables = await GetTablesOrderedByDependencies(connection);
            
            // Backup de las tablas
            foreach (var table in tables)
            {
                await BackupTable(connection, table, backupContent);
            }
            
            // Backup de triggers
            await BackupTriggers(connection, databaseName, backupContent);
            
            // Restaurar configuraciones
            backupContent.AppendLine("/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;");
            backupContent.AppendLine("/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;");
            backupContent.AppendLine("/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;");
            backupContent.AppendLine("/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;");
            backupContent.AppendLine("/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;");
            backupContent.AppendLine("/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;");
            backupContent.AppendLine("/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;");
            backupContent.AppendLine("/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;");
            backupContent.AppendLine();
            backupContent.AppendLine("-- Dump completed");
            
            return Encoding.UTF8.GetBytes(backupContent.ToString());
        }

        private async Task<List<string>> GetTablesOrderedByDependencies(MySqlConnection connection)
        {
            var tables = new List<string>();
            var dependencies = new Dictionary<string, List<string>>();
            
            // Obtener todas las tablas
            var tablesCommand = new MySqlCommand("SHOW TABLES", connection);
            using (var reader = await tablesCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var tableName = reader.GetString(0);
                    tables.Add(tableName);
                    dependencies[tableName] = new List<string>();
                }
            }
            
            // Obtener las dependencias de foreign keys
            var foreignKeysQuery = @"
                SELECT 
                    TABLE_NAME,
                    REFERENCED_TABLE_NAME
                FROM 
                    INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                WHERE 
                    REFERENCED_TABLE_SCHEMA = DATABASE()
                    AND REFERENCED_TABLE_NAME IS NOT NULL";
            
            var fkCommand = new MySqlCommand(foreignKeysQuery, connection);
            using (var reader = await fkCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var tableName = reader.GetString("TABLE_NAME");
                    var referencedTable = reader.GetString("REFERENCED_TABLE_NAME");
                    
                    if (dependencies.ContainsKey(tableName) && !dependencies[tableName].Contains(referencedTable))
                    {
                        dependencies[tableName].Add(referencedTable);
                    }
                }
            }
            
            // Ordenar topológicamente (tablas sin dependencias primero)
            var orderedTables = new List<string>();
            var visited = new HashSet<string>();
            var visiting = new HashSet<string>();
            
            foreach (var table in tables)
            {
                if (!visited.Contains(table))
                {
                    await TopologicalSort(table, dependencies, orderedTables, visited, visiting);
                }
            }
            
            return orderedTables;
        }
        
        private async Task TopologicalSort(string table, Dictionary<string, List<string>> dependencies, 
            List<string> result, HashSet<string> visited, HashSet<string> visiting)
        {
            if (visiting.Contains(table))
                return; // Evitar ciclos
                
            if (visited.Contains(table))
                return;
                
            visiting.Add(table);
            
            if (dependencies.ContainsKey(table))
            {
                foreach (var dependency in dependencies[table])
                {
                    await TopologicalSort(dependency, dependencies, result, visited, visiting);
                }
            }
            
            visiting.Remove(table);
            visited.Add(table);
            result.Add(table);
        }

        private async Task BackupTriggers(MySqlConnection connection, string databaseName, StringBuilder backupContent)
        {
            backupContent.AppendLine("-- Triggers");
            
            var triggersQuery = "SHOW TRIGGERS";
            var triggersCommand = new MySqlCommand(triggersQuery, connection);
            
            using var reader = await triggersCommand.ExecuteReaderAsync();
            var triggers = new List<(string name, string table)>();
            
            while (await reader.ReadAsync())
            {
                var triggerName = reader.GetString("Trigger");
                var tableName = reader.GetString("Table");
                triggers.Add((triggerName, tableName));
            }
            reader.Close();
            
            foreach (var (triggerName, tableName) in triggers)
            {
                backupContent.AppendLine($"-- Trigger: {triggerName}");
                
                var showCreateTriggerCommand = new MySqlCommand($"SHOW CREATE TRIGGER `{triggerName}`", connection);
                using var triggerReader = await showCreateTriggerCommand.ExecuteReaderAsync();
                
                if (await triggerReader.ReadAsync())
                {
                    var createStatement = triggerReader.GetString("SQL Original Statement");
                    backupContent.AppendLine($"DROP TRIGGER IF EXISTS `{triggerName}`;");
                    backupContent.AppendLine("DELIMITER ;;");
                    backupContent.AppendLine(createStatement + " ;;");
                    backupContent.AppendLine("DELIMITER ;");
                    backupContent.AppendLine();
                }
            }
        }
        
        private async Task BackupTable(MySqlConnection connection, string tableName, StringBuilder backupContent)
        {
            // Estructura de la tabla
            backupContent.AppendLine($"--");
            backupContent.AppendLine($"-- Table structure for table `{tableName}`");
            backupContent.AppendLine($"--");
            backupContent.AppendLine();
            backupContent.AppendLine($"DROP TABLE IF EXISTS `{tableName}`;");
            backupContent.AppendLine($"/*!40101 SET @saved_cs_client     = @@character_set_client */;");
            backupContent.AppendLine($"/*!50503 SET character_set_client = utf8mb4 */;");
            
            var showCreateCommand = new MySqlCommand($"SHOW CREATE TABLE `{tableName}`", connection);
            using (var reader = await showCreateCommand.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    backupContent.AppendLine(reader.GetString(1) + ";");
                }
            }
            
            backupContent.AppendLine($"/*!40101 SET character_set_client = @saved_cs_client */;");
            backupContent.AppendLine();
            
            // Contar registros REAL con COUNT(*)
            var countCommand = new MySqlCommand($"SELECT COUNT(*) FROM `{tableName}`", connection);
            var expectedRowCount = Convert.ToInt64(await countCommand.ExecuteScalarAsync());
            
            backupContent.AppendLine($"-- Expected rows for table `{tableName}`: {expectedRowCount}");
            
            if (expectedRowCount > 0)
            {
                backupContent.AppendLine($"--");
                backupContent.AppendLine($"-- Dumping data for table `{tableName}` (Expected: {expectedRowCount} rows)");
                backupContent.AppendLine($"--");
                backupContent.AppendLine();
                backupContent.AppendLine($"LOCK TABLES `{tableName}` WRITE;");
                backupContent.AppendLine($"/*!40000 ALTER TABLE `{tableName}` DISABLE KEYS */;");
                
                // Datos de la tabla - SIN LOTES, PROCESAMIENTO SIMPLE
                var dataCommand = new MySqlCommand($"SELECT * FROM `{tableName}`", connection);
                dataCommand.CommandTimeout = 300; // 5 minutos timeout
                
                using var dataReader = await dataCommand.ExecuteReaderAsync();
                
                var columnNames = new string[dataReader.FieldCount];
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    columnNames[i] = dataReader.GetName(i);
                }
                
                var actualRowsProcessed = 0L;
                var allInsertStatements = new List<string>();
                
                // PROCESAR FILA POR FILA - SIN LOTES
                while (await dataReader.ReadAsync())
                {
                    var values = new string[dataReader.FieldCount];
                    bool rowProcessedSuccessfully = true;
                    
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        try
                        {
                            if (dataReader.IsDBNull(i))
                            {
                                values[i] = "NULL";
                            }
                            else
                            {
                                var value = dataReader.GetValue(i);
                                values[i] = FormatValueForSql(value);
                            }
                        }
                        catch (Exception ex)
                        {
                            backupContent.AppendLine($"-- Error processing column {columnNames[i]} in row {actualRowsProcessed + 1}: {ex.Message}");
                            values[i] = "NULL"; // Valor por defecto en caso de error
                            rowProcessedSuccessfully = false;
                        }
                    }
                    
                    if (rowProcessedSuccessfully)
                    {
                        // INSERTAR FILA INDIVIDUAL - SIN AGRUPAR
                        var insertStatement = $"INSERT INTO `{tableName}` (`{string.Join("`,`", columnNames)}`) VALUES ({string.Join(",", values)});";
                        allInsertStatements.Add(insertStatement);
                    }
                    else
                    {
                        backupContent.AppendLine($"-- Skipped row {actualRowsProcessed + 1} due to processing errors");
                    }
                    
                    actualRowsProcessed++;
                }
                
                // Escribir TODOS los INSERT statements
                foreach (var insertStatement in allInsertStatements)
                {
                    backupContent.AppendLine(insertStatement);
                }
                
                backupContent.AppendLine($"-- Successfully processed {actualRowsProcessed} rows for table `{tableName}`");
                backupContent.AppendLine($"-- Generated {allInsertStatements.Count} INSERT statements");
                
                if (actualRowsProcessed != expectedRowCount)
                {
                    backupContent.AppendLine($"-- WARNING: Expected {expectedRowCount} rows but processed {actualRowsProcessed} rows!");
                }
                
                backupContent.AppendLine($"/*!40000 ALTER TABLE `{tableName}` ENABLE KEYS */;");
                backupContent.AppendLine("UNLOCK TABLES;");
            }
            else
            {
                backupContent.AppendLine($"-- Table `{tableName}` is empty (0 rows)");
            }
            
            backupContent.AppendLine();
        }

        private static string FormatValueForSql(object? value)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                    return "NULL";

                return value switch
                {
                    // Strings - escape characters properly
                    string s when string.IsNullOrEmpty(s) => "''",
                    string s => $"'{MySqlHelper.EscapeString(s)}'",
                    
                    // Characters
                    char c => $"'{MySqlHelper.EscapeString(c.ToString())}'",
                    
                    // DateTime types
                    DateTime dt when dt == DateTime.MinValue || dt == DateTime.MaxValue => "NULL",
                    DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
                    DateOnly d => $"'{d:yyyy-MM-dd}'",
                    TimeOnly t => $"'{t:HH:mm:ss}'",
                    TimeSpan ts => $"'{ts:hh\\:mm\\:ss}'",
                    
                    // Boolean
                    bool b => b ? "1" : "0",
                    
                    // Binary data
                    byte[] bytes when bytes.Length == 0 => "''",
                    byte[] bytes => $"0x{Convert.ToHexString(bytes)}",
                    
                    // Numeric types - use invariant culture and handle special values
                    decimal dec => dec.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    float f when float.IsNaN(f) || float.IsInfinity(f) => "NULL",
                    float f => f.ToString("R", System.Globalization.CultureInfo.InvariantCulture),
                    double d when double.IsNaN(d) || double.IsInfinity(d) => "NULL",  
                    double d => d.ToString("R", System.Globalization.CultureInfo.InvariantCulture),
                    
                    // Integer types
                    sbyte sb => sb.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    byte b => b.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    short s => s.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ushort us => us.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    int i => i.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    uint ui => ui.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    long l => l.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ulong ul => ul.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    
                    // GUID
                    Guid g when g == Guid.Empty => "NULL",
                    Guid g => $"'{g}'",
                    
                    // Enum - safe conversion
                    Enum e => Convert.ToInt64(e).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    
                    // Default - safe string conversion
                    _ => $"'{MySqlHelper.EscapeString(value.ToString() ?? "")}'"
                };
            }
            catch (Exception)
            {
                // Si todo falla, intentar conversión básica a string
                try
                {
                    var stringValue = value?.ToString() ?? "";
                    return $"'{MySqlHelper.EscapeString(stringValue)}'";
                }
                catch
                {
                    // Último recurso - devolver NULL
                    return "NULL";
                }
            }
        }
    }
}