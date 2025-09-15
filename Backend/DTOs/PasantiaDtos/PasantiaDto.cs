namespace Backend.DTOs.PasantiaDtos
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
        public string? DniTutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public string? DniTutorFacultad { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? FrecuenciaPago { get; set; }
        public string? Observaciones { get; set; }
        public string? TramiteSudocu { get; set; }
        public int? HorasSemanales { get; set; }
        public string? AreaTrabajo { get; set; }
        
        // Propiedades calculadas - no se almacenan en BD
        public string Estado => CalcularEstado(); // Calculado según fecha fin
        
        private string CalcularEstado()
        {
            if (!FechaFin.HasValue) return "Sin fecha de fin";
            
            var fechaActual = DateOnly.FromDateTime(DateTime.Now);
            if (FechaFin.Value <= fechaActual)
                return "Finalizada";
                
            var diasRestantes = FechaFin.Value.DayNumber - fechaActual.DayNumber;
            if (diasRestantes <= 30)
                return $"Por vencer ({diasRestantes} días)";
                
            return "Activa";
        }
    }
}
