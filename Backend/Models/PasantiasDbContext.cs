using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Backend.Models;

public partial class PasantiasDbContext : DbContext
{
    public PasantiasDbContext()
    {
    }

    public PasantiasDbContext(DbContextOptions<PasantiasDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditoria> Auditoria { get; set; }

    public virtual DbSet<Convenio> Convenios { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pasantia> Pasantias { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=dbpasantias.mysql.database.azure.com;port=3306;userid=myadmin;password=SBE_facet2025;database=pasantias_db;sslmode=Required", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

    modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PRIMARY");

            entity.ToTable("auditoria");

            entity.HasIndex(e => e.IdUsuario, "id_usuario");

            entity.Property(e => e.IdAuditoria).HasColumnName("id_auditoria");
            entity.Property(e => e.DatosAnteriores)
                .HasColumnType("text")
                .HasColumnName("datos_anteriores");
            entity.Property(e => e.DatosNuevos)
                .HasColumnType("text")
                .HasColumnName("datos_nuevos");
            entity.Property(e => e.FechaOperacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_operacion");
            entity.Property(e => e.FuncionLlamada)
                .HasMaxLength(100)
                .HasColumnName("funcion_llamada");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.TablaAfectada)
                .HasMaxLength(100)
                .HasColumnName("tabla_afectada");
            entity.Property(e => e.TipoOperacion)
                .HasColumnType("enum('INSERT','UPDATE','DELETE','LOGIN','LOGOUT')")
                .HasColumnName("tipo_operacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("auditoria_ibfk_1");
        });

        modelBuilder.Entity<Convenio>(entity =>
        {
            entity.HasKey(e => e.IdConvenio).HasName("PRIMARY");

            entity.ToTable("convenios");

            entity.HasIndex(e => e.IdEmpresa, "id_empresa");

            entity.Property(e => e.IdConvenio).HasColumnName("id_convenio");
            entity.Property(e => e.DocRepresentanteEmpresa)
                .HasMaxLength(255)
                .HasColumnName("doc_representante_empresa");
            entity.Property(e => e.DocRepresentanteFacultad)
                .HasMaxLength(255)
                .HasColumnName("doc_representante_facultad");
            entity.Property(e => e.DomicilioLegal)
                .HasMaxLength(255)
                .HasColumnName("domicilio_legal");
            entity.Property(e => e.FechaCaducidad).HasColumnName("fecha_caducidad");
            entity.Property(e => e.FechaFirma).HasColumnName("fecha_firma");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.NroAcuerdoMarco).HasColumnName("nro_acuerdo_marco");
            entity.Property(e => e.RepresentanteEmpresa)
                .HasMaxLength(255)
                .HasColumnName("representante_empresa");
            entity.Property(e => e.RepresentanteFacultad)
                .HasMaxLength(255)
                .HasColumnName("representante_facultad");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Convenios)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("convenios_ibfk_1");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PRIMARY");

            entity.ToTable("empresas");

            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .HasColumnName("celular");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(255)
                .HasColumnName("correo_electronico");
            entity.Property(e => e.Eliminado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("eliminado");
            entity.Property(e => e.Encargado)
                .HasMaxLength(255)
                .HasColumnName("encargado");
            entity.Property(e => e.FechaEliminacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_eliminacion");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.Sudocu)
                .HasMaxLength(255)
                .HasColumnName("sudocu");
            entity.Property(e => e.TipoContrato)
                .HasMaxLength(255)
                .HasColumnName("tipo_contrato");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.IdEstudiante).HasName("PRIMARY");

            entity.ToTable("estudiantes");

            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.AreaTrabajo)
                .HasMaxLength(100)
                .HasColumnName("area_trabajo");
            entity.Property(e => e.Carrera)
                .HasColumnType("enum('AGRIMENSURA','INGENIERÍA AZUCARERA','INGENIERÍA BIOMÉDICA','INGENIERÍA CIVIL','INGENIERÍA EN COMPUTACIÓN','INGENIERÍA EN INFORMÁTICA','INGENIERÍA ELÉCTRICA','INGENIERÍA ELECTRÓNICA','INGENIERÍA GEODÉSICA Y GEOFÍSICA','INGENIERÍA INDUSTRIAL','INGENIERÍA MECÁNICA','INGENIERÍA QUÍMICA','LICENCIATURA EN FÍSICA','LICENCIATURA EN MATEMÁTICA','LICENCIATURA EN INFORMÁTICA','DISEÑO DE ILUMINACIÓN','PROGRAMADOR UNIVERSITARIO','TECNICATURA UNIVERSITARIA EN TECNOLOGÍA','AZUCARERA E INDUSTRIAS DERIVADAS','TECNICATURA UNIVERSITARIA EN FÍSICA','TECNICATURA UNIVERSITARIA EN FÍSICA AMBIENTAL', 'OTRA')")
                .HasColumnName("carrera");
            entity.Property(e => e.Documento)
                .HasMaxLength(50)
                .HasColumnName("documento");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(255)
                .HasColumnName("domicilio");
            entity.Property(e => e.Eliminado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("eliminado");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.FechaEliminacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_eliminacion");
            entity.Property(e => e.Libreta)
                .HasMaxLength(50)
                .HasColumnName("libreta");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PRIMARY");

            entity.ToTable("pagos");

            entity.HasIndex(e => e.IdPasantia, "idx_pasantia");

            entity.Property(e => e.IdPago).HasColumnName("id_pago");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdPasantia).HasColumnName("id_pasantia");
            entity.Property(e => e.Monto)
                .HasPrecision(10, 2)
                .HasColumnName("monto");
            entity.Property(e => e.Observaciones)
                .HasColumnType("text")
                .HasColumnName("observaciones");
            entity.Property(e => e.Pagado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("pagado");

            entity.HasOne(d => d.IdPasantiaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdPasantia)
                .HasConstraintName("pagos_ibfk_1");
        });

        modelBuilder.Entity<Pasantia>(entity =>
        {
            entity.HasKey(e => e.IdPasantia).HasName("PRIMARY");

            entity.ToTable("pasantias");

            entity.HasIndex(e => e.IdConvenio, "idx_convenio");

            entity.HasIndex(e => e.IdEstudiante, "idx_estudiante");

            entity.Property(e => e.IdPasantia).HasColumnName("id_pasantia");
            entity.Property(e => e.Art)
                .HasMaxLength(100)
                .HasColumnName("art");
            entity.Property(e => e.AsignacionMensual)
                .HasPrecision(10, 2)
                .HasColumnName("asignacion_mensual");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.FrecuenciaPago)
                .HasColumnType("enum('Mensual','Trimestral','Semestral','Anual')")
                .HasColumnName("frecuencia_pago");
            entity.Property(e => e.IdConvenio).HasColumnName("id_convenio");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.MontoPago)
                .HasPrecision(10)
                .HasColumnName("monto_pago");
            entity.Property(e => e.ObraSocial)
                .HasMaxLength(100)
                .HasColumnName("obra_social");
            entity.Property(e => e.Observaciones)
                .HasColumnType("text")
                .HasColumnName("observaciones");
            entity.Property(e => e.TipoAcuerdo)
                .HasColumnType("enum('Pasantia','PPS','otro')")
                .HasColumnName("tipo_acuerdo");
            entity.Property(e => e.TutorEmpresa)
                .HasMaxLength(100)
                .HasColumnName("tutor_empresa");
            entity.Property(e => e.TutorFacultad)
                .HasMaxLength(100)
                .HasColumnName("tutor_facultad");
            entity.Property(e => e.DniTutorFacultad)
                .HasMaxLength(100)
                .HasColumnName("dni_tutor_facultad");
            entity.Property(e => e.Sudocu)
                .HasMaxLength(255)
                .HasColumnName("sudocu");

            entity.HasOne(d => d.IdConvenioNavigation).WithMany(p => p.Pasantia)
                .HasForeignKey(d => d.IdConvenio)
                .HasConstraintName("pasantias_ibfk_2");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Pasantia)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("pasantias_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "correo").IsUnique();

            entity.HasIndex(e => e.NombreUsuario, "idx_usuario").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.ContrasenaHash)
                .HasMaxLength(255)
                .HasColumnName("contrasena_hash");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.Eliminado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("eliminado");
            entity.Property(e => e.FechaEliminacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_eliminacion");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.Rol)
                .HasColumnType("enum('admin','empresa','estudiante')")
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
