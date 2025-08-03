namespace Backend.DTOs
{
    using System.ComponentModel.DataAnnotations;

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
}
