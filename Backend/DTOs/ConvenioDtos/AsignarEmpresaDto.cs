using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.ConvenioDtos
{
    public class AsignarEmpresaDto : IValidatableObject
    {
        public int ConvenioId { get; set; }
        public int EmpresaId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ConvenioDtoValidator.ValidateAsignarEmpresa(ConvenioId, EmpresaId);
        }
    }
}
