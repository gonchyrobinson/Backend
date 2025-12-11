namespace Backend.DTOs.PasantiaDtos
{
    public class PasantiaShowTableDto
    {
        public int IdPasantia { get; set; }
        public string? TramiteSudocu { get; set; }
        public string Estudiante { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string TipoAcuerdo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? TutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
    }
}
