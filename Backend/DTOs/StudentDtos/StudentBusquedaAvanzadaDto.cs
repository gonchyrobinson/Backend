using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

public class StudentBusquedaAvanzadaDto : IValidatableObject
{
    public string? Documento { get; set; }
    public string? Carrera { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return StudentDtoValidator.ValidateStudentSearch(Documento, Carrera);
    }
}
