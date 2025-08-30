namespace Backend.DTOs.PasantiaDtos
{
    public class PasantiaShowTableDto
    {
        public string Tramite { get; set; } = string.Empty;
        public string Estudiante { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string TipoAcuerdo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
    }
}
