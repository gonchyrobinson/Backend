using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.ConvenioDtos
{
    public class ConvenioEmpresaFiltroDto : IValidatableObject
    {
        public string? NombreEmpresa { get; set; }
        public string? NumeroAcuerdoMarco { get; set; }
        public bool? Vigencia { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ConvenioDtoValidator.ValidateConvenioEmpresaFiltro(
                NombreEmpresa, NumeroAcuerdoMarco, Vigencia);
        }
    }
}
