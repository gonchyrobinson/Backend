using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.AuditoriaDtos
{
    public class AuditoriaUpdateDto : IValidatableObject
    {
        public int IdAuditoria { get; set; }
        public int? IdUsuario { get; set; }
        public string? TablaAfectada { get; set; }
        public string? TipoOperacion { get; set; }
        public string? DatosAnteriores { get; set; }
        public string? DatosNuevos { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string? FuncionLlamada { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validaciones básicas para auditoría update
            if (IdAuditoria <= 0)
                yield return new ValidationResult("IdAuditoria debe ser mayor a 0.", new[] { nameof(IdAuditoria) });

            if (!string.IsNullOrEmpty(TablaAfectada) && TablaAfectada.Length > 100)
                yield return new ValidationResult("TablaAfectada no puede exceder 100 caracteres.", new[] { nameof(TablaAfectada) });

            if (!string.IsNullOrEmpty(TipoOperacion) && TipoOperacion.Length > 50)
                yield return new ValidationResult("TipoOperacion no puede exceder 50 caracteres.", new[] { nameof(TipoOperacion) });

            if (!string.IsNullOrEmpty(FuncionLlamada) && FuncionLlamada.Length > 100)
                yield return new ValidationResult("FuncionLlamada no puede exceder 100 caracteres.", new[] { nameof(FuncionLlamada) });
        }
    }
}
