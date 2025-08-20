namespace Backend.DTOs
{
    // DTO para filtros de búsqueda avanzada de pasantías
    public class PasantiaBusquedaAvanzadaDto
    {
        public string? Tramite { get; set; }
        public string? ObraSocial { get; set; }
        public string? Art { get; set; }
        public string? TutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public string? TipoAcuerdo { get; set; }
        public DateOnly? FechaInicioDesde { get; set; }
        public DateOnly? FechaInicioHasta { get; set; }
        public DateOnly? FechaFinDesde { get; set; }
        public DateOnly? FechaFinHasta { get; set; }
        public int? IdEstudiante { get; set; }
        public int? IdConvenio { get; set; }
        public string? Estado { get; set; }
    }

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
        public string? DniTutorFacultad { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public string? Tramite { get; set; }
        public DateOnly? FechaFin { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? FrecuenciaPago { get; set; }
        public decimal MontoPago { get; set; }
        public string? Observaciones { get; set; }
        public string? Sudocu { get; set; }
        public int? HorasSemanales { get; set; }
        
        // Propiedades calculadas - no se almacenan en BD
        public string? AreaTrabajo { get; set; } // Viene del estudiante asociado
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

    public class PasantiaCreateDto
    {
        public int? IdEstudiante { get; set; }
        public int? IdConvenio { get; set; }
        public decimal? AsignacionMensual { get; set; }
        public string? ObraSocial { get; set; }
        public string? Art { get; set; }
        public string? TutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public string? DniTutorFacultad { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? FrecuenciaPago { get; set; }
        public string? Observaciones { get; set; }
        public string? Sudocu { get; set; }
        public decimal MontoPago { get; set; }
        public int? HorasSemanales { get; set; }
        // areaTrabajo y estado no se incluyen - se calculan automáticamente
    }

    public class PasantiaDetalleDto
    {
        public PasantiaDto Pasantia { get; set; } = null!;
        public StudentDto? Estudiante { get; set; }
        public ConvenioDto? Convenio { get; set; }
    }
}
