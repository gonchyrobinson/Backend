using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class AuditoriaDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidateAuditoriaBuscar(
            DateTime? fechaDesde, DateTime? fechaHasta, string? usuarioNombre, string? accion)
        {
            // Validar rango de fechas
            if (fechaDesde.HasValue && fechaHasta.HasValue && fechaDesde.Value > fechaHasta.Value)
                yield return CreateValidationResult("FechaDesde debe ser menor o igual a FechaHasta.", nameof(fechaDesde));

            // Validar que las fechas no sean muy antiguas (más de 5 años)
            if (fechaDesde.HasValue && fechaDesde.Value < DateTime.Now.AddYears(-5))
                yield return CreateValidationResult("FechaDesde no puede ser anterior a 5 años.", nameof(fechaDesde));

            // Validar que las fechas no sean futuras
            if (fechaDesde.HasValue && fechaDesde.Value > DateTime.Now.AddDays(1))
                yield return CreateValidationResult("FechaDesde no puede ser futura.", nameof(fechaDesde));

            if (fechaHasta.HasValue && fechaHasta.Value > DateTime.Now.AddDays(1))
                yield return CreateValidationResult("FechaHasta no puede ser futura.", nameof(fechaHasta));

            // Validaciones de longitud
            if (!string.IsNullOrEmpty(usuarioNombre) && usuarioNombre.Length > 100)
                yield return CreateValidationResult("El nombre de usuario no puede exceder 100 caracteres.", nameof(usuarioNombre));

            if (!string.IsNullOrEmpty(accion) && accion.Length > 50)
                yield return CreateValidationResult("La acción no puede exceder 50 caracteres.", nameof(accion));
        }
    }
}
