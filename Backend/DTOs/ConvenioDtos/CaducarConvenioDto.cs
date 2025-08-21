using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.ConvenioDtos
{
    public class CaducarConvenioDto : IValidatableObject
    {
        public string? FechaCaducidad { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ConvenioDtoValidator.ValidateCaducarConvenio(FechaCaducidad);
        }
    }
}
