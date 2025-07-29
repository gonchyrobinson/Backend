-- =====================================================
-- Script de inicialización de la base de datos
-- =====================================================

-- Configurar el modo SQL para ser más permisivo
SET sql_mode = 'NO_ENGINE_SUBSTITUTION';

-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS pasantias_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

-- Usar la base de datos
USE pasantias_db;

-- Crear usuario si no existe
CREATE USER IF NOT EXISTS 'appuser'@'%' IDENTIFIED BY 'TuPasswordSeguro123!';

-- Otorgar permisos al usuario
GRANT ALL PRIVILEGES ON pasantias_db.* TO 'appuser'@'%';
GRANT ALL PRIVILEGES ON pasantias_db.* TO 'root'@'%';

-- Aplicar cambios
FLUSH PRIVILEGES;

-- Crear tabla de estudiantes si no existe
CREATE TABLE IF NOT EXISTS Estudiantes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Carrera VARCHAR(100) NOT NULL,
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insertar datos de ejemplo
INSERT INTO Estudiantes (Nombre, Email, Carrera) VALUES
('Juan Pérez', 'juan.perez@email.com', 'Ingeniería Informática'),
('María García', 'maria.garcia@email.com', 'Administración de Empresas'),
('Carlos López', 'carlos.lopez@email.com', 'Contabilidad')
ON DUPLICATE KEY UPDATE Nombre = VALUES(Nombre);

-- Verificar que la tabla se creó correctamente
SELECT 'Base de datos inicializada correctamente' AS Estado;