using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.AuditoriaDtos
{
    public class AuditoriaBuscarDto : IValidatableObject
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? Accion { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return AuditoriaDtoValidator.ValidateAuditoriaBuscar(FechaDesde, FechaHasta, UsuarioNombre, Accion);
        }
    }
}
