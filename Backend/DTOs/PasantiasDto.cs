namespace Backend.DTOs
{
    public class PasantiaDto
    {
        public int IdPasantia { get; set; }
        public int? IdEstudiante { get; set; }
        public int? IdConvenio { get; set; }
        public decimal? AsignacionMensual { get; set; }
        public string? ObraSocial { get; set; }
        public string? Art { get; set; }
        public string? TutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public string? Expediente { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? FrecuenciaPago { get; set; }
        public decimal MontoPago { get; set; }
        public string? Observaciones { get; set; }
        public string? AreaTrabajo { get; set; }
        public string? Estado { get; set; }
    }

    public class PasantiaCreateDto
    {
        public int? IdEstudiante { get; set; }
        public int? IdConvenio { get; set; }
        public decimal? AsignacionMensual { get; set; }
        public string? ObraSocial { get; set; }
        public string? Art { get; set; }
        public string? TutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public string? Expediente { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? FrecuenciaPago { get; set; }
        public string? Observaciones { get; set; }
        public string? AreaTrabajo { get; set; }
        public string? Estado { get; set; }
        public decimal MontoPago { get; set; }
    }

    public class PasantiaDetalleDto
    {
        public PasantiaDto Pasantia { get; set; } = null!;
        public StudentDto? Estudiante { get; set; }
        public ConvenioDto? Convenio { get; set; }
    }
}
