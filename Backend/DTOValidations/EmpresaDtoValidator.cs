using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class EmpresaDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidateEmpresa(
            string? nombre, string? correoElectronico,
            bool isCreate = false)
        {
            // Validaciones requeridas
            if (IsNullOrWhiteSpace(nombre))
                yield return CreateValidationResult("El nombre de la empresa es obligatorio.", nameof(nombre));
            // Validaciones de formato
            if (!string.IsNullOrEmpty(correoElectronico) && !IsValidEmail(correoElectronico))
                yield return CreateValidationResult("El correo electrónico debe tener un formato válido.", nameof(correoElectronico));

            // Validaciones de longitud
            if (!string.IsNullOrEmpty(nombre) && nombre.Length > 255)
                yield return CreateValidationResult("El nombre no puede exceder 255 caracteres.", nameof(nombre));

            if (!string.IsNullOrEmpty(correoElectronico) && correoElectronico.Length > 255)
                yield return CreateValidationResult("El correo electrónico no puede exceder 255 caracteres.", nameof(correoElectronico));
        }

        public static IEnumerable<ValidationResult> ValidateEmpresaBusqueda(
            string? nombre)
        {
            // Currently no advanced filters; keep placeholder for future rules
            yield break;
        }
    }
}
