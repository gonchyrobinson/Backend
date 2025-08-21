using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.StudentDtos
{
    public class StudentDto : IValidatableObject
    {
        public int IdEstudiante { get; set; }
        public string? Apellido { get; set; }
        public string? Nombre { get; set; }
        public string? Documento { get; set; }
        public string? Domicilio { get; set; }
        public string? Carrera { get; set; }
        public string? AreaTrabajo { get; set; }
        public string? Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return StudentDtoValidator.ValidateStudent(Nombre, Apellido, Documento, Email, Carrera, Domicilio, isCreate: false);
        }
    }
}
