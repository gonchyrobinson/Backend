using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.EmpresaDtos
{
    public class EmpresaBusquedaAvanzadaDto : IValidatableObject
    {
        public string? Nombre { get; set; }

        public bool? Vigencia { get; set; }

        [RegularExpression("^(PPS|Pasantia|otro)$", ErrorMessage = "El tipo de contrato debe ser PPS, Pasantia u otro")]
        public string? TipoContrato { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return EmpresaDtoValidator.ValidateEmpresaBusqueda(
                Nombre, Vigencia, TipoContrato);
        }
    }
}
