using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.EmpresaDtos
{
    public class EmpresaBusquedaAvanzadaDto : IValidatableObject
    {
        public string? Nombre { get; set; }

        public bool? Vigencia { get; set; }

        public string? TipoContrato { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaInicioDesde debe ser una fecha válida.")]
        public DateOnly? FechaInicioDesde { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaInicioHasta debe ser una fecha válida.")]
        public DateOnly? FechaInicioHasta { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaFinDesde debe ser una fecha válida.")]
        public DateOnly? FechaFinDesde { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaFinHasta debe ser una fecha válida.")]
        public DateOnly? FechaFinHasta { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return EmpresaDtoValidator.ValidateEmpresaBusqueda(
                Nombre, Vigencia, TipoContrato,
                FechaInicioDesde, FechaInicioHasta, FechaFinDesde, FechaFinHasta);
        }
    }
}
