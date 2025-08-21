using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.PagosDtos
{
    public class MarcarPagoDto : IValidatableObject
    {
        public int IdPago { get; set; }
        public DateOnly? FechaPago { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return PagoDtoValidator.ValidateMarcarPago(IdPago, FechaPago);
        }
    }
}
