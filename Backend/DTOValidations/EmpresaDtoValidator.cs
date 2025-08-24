using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class EmpresaDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidateEmpresa(
            string? nombre, string? vigencia, DateOnly? fechaInicio, DateOnly? fechaFin,
            string? tipoContrato, string? encargado, string? celular, string? correoElectronico,
            bool isCreate = false)
        {
            // Validaciones requeridas
            if (IsNullOrWhiteSpace(nombre))
                yield return CreateValidationResult("El nombre de la empresa es obligatorio.", nameof(nombre));

            if (IsNullOrWhiteSpace(tipoContrato))
                yield return CreateValidationResult("El tipo de contrato es obligatorio.", nameof(tipoContrato));

            // Validaciones de formato
            if (!CommonValidations.EsVigenciaValida(vigencia))
                yield return CreateValidationResult("La vigencia debe ser 'vigente', 'no_vigente' o vacía.", nameof(vigencia));

            if (!CommonValidations.EsTipoContratoValido(tipoContrato))
                yield return CreateValidationResult("El tipo de contrato debe ser válido.", nameof(tipoContrato));

            if (!string.IsNullOrEmpty(celular) && !IsValidPhone(celular))
                yield return CreateValidationResult("El celular debe tener un formato válido.", nameof(celular));

            if (!string.IsNullOrEmpty(correoElectronico) && !IsValidEmail(correoElectronico))
                yield return CreateValidationResult("El correo electrónico debe tener un formato válido.", nameof(correoElectronico));

            // Validaciones de fechas
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio.Value > fechaFin.Value)
                yield return CreateValidationResult("La fecha de inicio debe ser menor o igual a la fecha de fin.", nameof(fechaInicio));

            // Validaciones de longitud
            if (!string.IsNullOrEmpty(nombre) && nombre.Length > 255)
                yield return CreateValidationResult("El nombre no puede exceder 255 caracteres.", nameof(nombre));

            if (!string.IsNullOrEmpty(encargado) && encargado.Length > 100)
                yield return CreateValidationResult("El encargado no puede exceder 100 caracteres.", nameof(encargado));

            if (!string.IsNullOrEmpty(correoElectronico) && correoElectronico.Length > 255)
                yield return CreateValidationResult("El correo electrónico no puede exceder 255 caracteres.", nameof(correoElectronico));
        }

        public static IEnumerable<ValidationResult> ValidateEmpresaBusqueda(
            string? nombre, bool? vigencia, string? tipoContrato)
        {
            // Validar tipo de contrato si se proporciona
            if (!string.IsNullOrEmpty(tipoContrato) && !CommonValidations.EsTipoContratoValido(tipoContrato))
                yield return CreateValidationResult("El tipo de contrato debe ser válido.", nameof(tipoContrato));
        }
    }
}
