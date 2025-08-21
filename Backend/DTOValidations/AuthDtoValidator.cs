using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class AuthDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidateLogin(
            string? username, string? password)
        {
            if (IsNullOrWhiteSpace(username))
                yield return CreateValidationResult("El nombre de usuario es obligatorio.", nameof(username));

            if (IsNullOrWhiteSpace(password))
                yield return CreateValidationResult("La contraseña es obligatoria.", nameof(password));

            // Validaciones de longitud
            if (!string.IsNullOrEmpty(username) && username.Length > 50)
                yield return CreateValidationResult("El nombre de usuario no puede exceder 50 caracteres.", nameof(username));

            if (!string.IsNullOrEmpty(password) && password.Length > 100)
                yield return CreateValidationResult("La contraseña no puede exceder 100 caracteres.", nameof(password));
        }

        public static IEnumerable<ValidationResult> ValidateRegister(
            string? username, string? email, string? password)
        {
            if (IsNullOrWhiteSpace(username))
                yield return CreateValidationResult("El nombre de usuario es obligatorio.", nameof(username));

            if (IsNullOrWhiteSpace(email))
                yield return CreateValidationResult("El email es obligatorio.", nameof(email));

            if (IsNullOrWhiteSpace(password))
                yield return CreateValidationResult("La contraseña es obligatoria.", nameof(password));

            // Validaciones de formato
            if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
                yield return CreateValidationResult("El email debe tener un formato válido.", nameof(email));

            // Validaciones de longitud y complejidad
            if (!string.IsNullOrEmpty(username) && username.Length < 3)
                yield return CreateValidationResult("El nombre de usuario debe tener al menos 3 caracteres.", nameof(username));

            if (!string.IsNullOrEmpty(username) && username.Length > 50)
                yield return CreateValidationResult("El nombre de usuario no puede exceder 50 caracteres.", nameof(username));

            if (!string.IsNullOrEmpty(password) && password.Length < 6)
                yield return CreateValidationResult("La contraseña debe tener al menos 6 caracteres.", nameof(password));

            if (!string.IsNullOrEmpty(password) && password.Length > 100)
                yield return CreateValidationResult("La contraseña no puede exceder 100 caracteres.", nameof(password));

            if (!string.IsNullOrEmpty(email) && email.Length > 255)
                yield return CreateValidationResult("El email no puede exceder 255 caracteres.", nameof(email));
        }
    }
}
