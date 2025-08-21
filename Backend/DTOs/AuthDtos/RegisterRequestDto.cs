using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.AuthDtos
{
    public class RegisterRequestDto : IValidatableObject
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return AuthDtoValidator.ValidateRegister(Username, Email, Password);
        }
    }
}
