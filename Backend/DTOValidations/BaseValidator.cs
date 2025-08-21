using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public abstract class BaseValidator
    {
        protected static ValidationResult CreateValidationResult(string message, string fieldName)
        {
            return new ValidationResult(message, new[] { fieldName });
        }

        protected static bool IsNullOrWhiteSpace(string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        protected static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        protected static bool IsValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            
            // Validación básica para teléfonos argentinos
            var cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            return cleanPhone.All(char.IsDigit) && cleanPhone.Length >= 8 && cleanPhone.Length <= 15;
        }
    }
}
