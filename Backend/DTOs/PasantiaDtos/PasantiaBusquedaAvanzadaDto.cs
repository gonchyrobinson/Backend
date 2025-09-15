using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.PasantiaDtos
{
    public class PasantiaBusquedaAvanzadaDto : IValidatableObject
    {
        public string? TramiteSudocu { get; set; }
        public string? Tipo { get; set; }
        public string? Estudiante { get; set; }
        public string? Empresa { get; set; }
        public bool? Vigente { get; set; }
        public string? Carrera { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return PasantiaDtoValidator.ValidatePasantiaBusqueda(
                TramiteSudocu, Tipo, Estudiante, Empresa, Vigente, Carrera);
        }
    }
}
