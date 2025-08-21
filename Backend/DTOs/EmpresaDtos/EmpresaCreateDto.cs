using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.EmpresaDtos
{
    public class EmpresaCreateDto : IValidatableObject
    {
        public string? Nombre { get; set; }

        public string? Vigencia { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaInicio debe ser una fecha válida.")]
        public DateOnly? FechaInicio { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaFin debe ser una fecha válida.")]
        public DateOnly? FechaFin { get; set; }

        public string? TipoContrato { get; set; }

        public string? Encargado { get; set; }

        public string? Celular { get; set; }

        public string? CorreoElectronico { get; set; }

        public string? Sudocu { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return EmpresaDtoValidator.ValidateEmpresa(
                Nombre, Vigencia, FechaInicio, FechaFin, TipoContrato,
                Encargado, Celular, CorreoElectronico, isCreate: true);
        }
    }
}
