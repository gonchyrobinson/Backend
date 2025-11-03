using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.AuthDtos
{
    public class UpdateUserRequestDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [MinLength(3, ErrorMessage = "El nombre de usuario debe tener al menos 3 caracteres")]
        [MaxLength(100, ErrorMessage = "El nombre de usuario no puede exceder 100 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "El correo debe tener un formato válido")]
        [MaxLength(255, ErrorMessage = "El correo no puede exceder 255 caracteres")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        [RegularExpression("^(admin|coordinador)$", ErrorMessage = "El rol debe ser 'admin' o 'coordinador'")]
        public string Rol { get; set; } = string.Empty;
    }
}

