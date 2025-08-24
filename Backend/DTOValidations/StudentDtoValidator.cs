using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class StudentDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidateStudent(
            string? nombre, string? apellido, string? documento, 
            string? email, string? carrera, string? domicilio, 
            bool isCreate = false)
        {
            // Validaciones requeridas para creación
            if (isCreate)
            {
                if (IsNullOrWhiteSpace(nombre))
                    yield return CreateValidationResult("El nombre es obligatorio.", nameof(nombre));
                
                if (IsNullOrWhiteSpace(apellido))
                    yield return CreateValidationResult("El apellido es obligatorio.", nameof(apellido));
                
                if (IsNullOrWhiteSpace(documento))
                    yield return CreateValidationResult("El documento es obligatorio.", nameof(documento));
                
                if (IsNullOrWhiteSpace(email))
                    yield return CreateValidationResult("El email es obligatorio.", nameof(email));
            }

            // Validaciones de formato
            if (!string.IsNullOrEmpty(documento) && !CommonValidations.EsDniValido(documento))
                yield return CreateValidationResult("El documento debe ser un DNI válido (7 u 8 dígitos).", nameof(documento));

            if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
                yield return CreateValidationResult("El email debe tener un formato válido.", nameof(email));

            if (!CommonValidations.EsCarreraValida(carrera))
                yield return CreateValidationResult("La carrera debe ser una de las opciones válidas.", nameof(carrera));

            // Validaciones de longitud
            if (!string.IsNullOrEmpty(nombre) && nombre.Length > 100)
                yield return CreateValidationResult("El nombre no puede exceder 100 caracteres.", nameof(nombre));

            if (!string.IsNullOrEmpty(apellido) && apellido.Length > 100)
                yield return CreateValidationResult("El apellido no puede exceder 100 caracteres.", nameof(apellido));

            if (!string.IsNullOrEmpty(domicilio) && domicilio.Length > 255)
                yield return CreateValidationResult("El domicilio no puede exceder 255 caracteres.", nameof(domicilio));
        }


        public static IEnumerable<ValidationResult> ValidateStudentSearch(
            string? documento, string? carrera)
        {
            // Para búsquedas, solo validar formato si se proporcionan
            if (!string.IsNullOrEmpty(documento) && !CommonValidations.EsDniValido(documento))
                yield return CreateValidationResult("El documento debe ser un DNI válido.", nameof(documento));

            if (!CommonValidations.EsCarreraValida(carrera))
                yield return CreateValidationResult("La carrera debe ser una de las opciones válidas.", nameof(carrera));
        }
    }
}
