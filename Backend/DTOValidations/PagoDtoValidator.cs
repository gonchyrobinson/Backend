using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class PagoDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidatePago(
            int? idPasantia, decimal? monto, DateOnly? fechaVencimiento,
            DateOnly? fechaPago, bool isCreate = false)
        {
            // Validaciones requeridas
            if (!idPasantia.HasValue || idPasantia <= 0)
                yield return CreateValidationResult("El ID de la pasantía es obligatorio.", nameof(idPasantia));

            if (!monto.HasValue || monto <= 0)
                yield return CreateValidationResult("El monto debe ser mayor a cero.", nameof(monto));

            if (!fechaVencimiento.HasValue)
                yield return CreateValidationResult("La fecha de vencimiento es obligatoria.", nameof(fechaVencimiento));

            // Validaciones de negocio
            if (fechaVencimiento.HasValue && fechaVencimiento < DateOnly.FromDateTime(DateTime.Now.AddDays(-365)))
                yield return CreateValidationResult("La fecha de vencimiento no puede ser anterior a un año atrás.", nameof(fechaVencimiento));

            // Validar fecha de pago si se proporciona
            if (fechaPago.HasValue && fechaPago > DateOnly.FromDateTime(DateTime.Now.AddDays(1)))
                yield return CreateValidationResult("La fecha de pago no puede ser futura.", nameof(fechaPago));

            // Validar que fecha de pago no sea muy anterior
            if (fechaPago.HasValue && fechaPago < DateOnly.FromDateTime(DateTime.Now.AddYears(-5)))
                yield return CreateValidationResult("La fecha de pago no puede ser anterior a 5 años.", nameof(fechaPago));

            // Validar monto máximo razonable
            if (monto.HasValue && monto > 1000000)
                yield return CreateValidationResult("El monto no puede exceder $1,000,000.", nameof(monto));
        }

        public static IEnumerable<ValidationResult> ValidateMarcarPago(
            int idPago, DateOnly? fechaPago)
        {
            if (idPago <= 0)
                yield return CreateValidationResult("El ID del pago debe ser positivo.", nameof(idPago));

            if (!fechaPago.HasValue)
                yield return CreateValidationResult("La fecha de pago es obligatoria.", nameof(fechaPago));

            if (fechaPago.HasValue && fechaPago > DateOnly.FromDateTime(DateTime.Now.AddDays(1)))
                yield return CreateValidationResult("La fecha de pago no puede ser futura.", nameof(fechaPago));
        }
    }
}
