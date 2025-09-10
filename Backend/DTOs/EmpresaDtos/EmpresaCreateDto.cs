using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.EmpresaDtos
{
    public class EmpresaCreateDto : IValidatableObject
    {
        public string? Nombre { get; set; }

        public string? CorreoElectronico { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return EmpresaDtoValidator.ValidateEmpresa(
                Nombre, CorreoElectronico, isCreate: true);
        }
    }
}
