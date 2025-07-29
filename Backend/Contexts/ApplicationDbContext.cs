using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Estudiantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración específica para Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Carrera).IsRequired().HasMaxLength(100);
                
                // Configuración específica para DateTime en MySQL
                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configuración adicional para MySQL
                optionsBuilder.UseMySql(
                    connectionString: "Server=mysql;Database=pasantias_db;User=appuser;Password=TuPasswordSeguro123!;Port=3306;CharSet=utf8mb4;",
                    serverVersion: new MySqlServerVersion(new Version(8, 0, 0))
                );
            }
        }
    }
}