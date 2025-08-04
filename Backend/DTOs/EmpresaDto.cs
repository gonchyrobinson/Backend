using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class EmpresaDto
    {
        public int IdEmpresa { get; set; }
        public string? Nombre { get; set; }
        public string? Vigencia { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }

        [Required]
        [RegularExpression("^(temporal|indefinido|otro)$", ErrorMessage = "TipoContrato debe ser 'temporal', 'indefinido' u 'otro'.")]
        public string? TipoContrato { get; set; }

        public string? Encargado { get; set; }
        public string? Celular { get; set; }
        public string? CorreoElectronico { get; set; }
        public DateOnly? Sudocu { get; set; }
    }

    public class EmpresaBusquedaAvanzadaDto
    {
        public string? Nombre { get; set; }
        public string? Vigencia { get; set; }
        public string? TipoContrato { get; set; }
        public DateOnly? FechaInicioDesde { get; set; }
        public DateOnly? FechaInicioHasta { get; set; }
        public DateOnly? FechaFinDesde { get; set; }
        public DateOnly? FechaFinHasta { get; set; }
    }
}
