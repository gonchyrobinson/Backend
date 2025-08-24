using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class ConvenioDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidateConvenio(
            int? idEmpresa, string? representanteEmpresa, string? docRepresentanteEmpresa,
            string? representanteFacultad, string? docRepresentanteFacultad,
            DateOnly? fechaFirma, DateOnly? fechaCaducidad, string? domicilioLegal,
            string? domicilioAlternativo, bool isCreate = false)
        {
            // Validaciones requeridas
            if (!idEmpresa.HasValue || idEmpresa <= 0)
                yield return CreateValidationResult("El ID de la empresa es obligatorio.", nameof(idEmpresa));

            if (IsNullOrWhiteSpace(representanteEmpresa))
                yield return CreateValidationResult("El representante de empresa es obligatorio.", nameof(representanteEmpresa));

            if (IsNullOrWhiteSpace(docRepresentanteEmpresa))
                yield return CreateValidationResult("El documento del representante de empresa es obligatorio.", nameof(docRepresentanteEmpresa));

            if (IsNullOrWhiteSpace(representanteFacultad))
                yield return CreateValidationResult("El representante de facultad es obligatorio.", nameof(representanteFacultad));

            if (IsNullOrWhiteSpace(docRepresentanteFacultad))
                yield return CreateValidationResult("El documento del representante de facultad es obligatorio.", nameof(docRepresentanteFacultad));

            if (!fechaFirma.HasValue)
                yield return CreateValidationResult("La fecha de firma es obligatoria.", nameof(fechaFirma));

            if (IsNullOrWhiteSpace(domicilioLegal))
                yield return CreateValidationResult("El domicilio legal es obligatorio.", nameof(domicilioLegal));

            // Validaciones de formato
            if (!string.IsNullOrEmpty(docRepresentanteEmpresa) && !CommonValidations.EsDniValido(docRepresentanteEmpresa))
                yield return CreateValidationResult("El documento del representante de empresa debe ser un DNI válido.", nameof(docRepresentanteEmpresa));

            if (!string.IsNullOrEmpty(docRepresentanteFacultad) && !CommonValidations.EsDniValido(docRepresentanteFacultad))
                yield return CreateValidationResult("El documento del representante de facultad debe ser un DNI válido.", nameof(docRepresentanteFacultad));

            // Validaciones de fechas
            if (fechaFirma.HasValue && fechaFirma < DateOnly.FromDateTime(DateTime.Now.AddYears(-10)))
                yield return CreateValidationResult("La fecha de firma no puede ser anterior a 10 años.", nameof(fechaFirma));

            if (fechaFirma.HasValue && fechaFirma > DateOnly.FromDateTime(DateTime.Now.AddDays(30)))
                yield return CreateValidationResult("La fecha de firma no puede ser más de 30 días en el futuro.", nameof(fechaFirma));

            if (fechaCaducidad.HasValue && fechaFirma.HasValue && fechaCaducidad <= fechaFirma)
                yield return CreateValidationResult("La fecha de caducidad debe ser posterior a la fecha de firma.", nameof(fechaCaducidad));

            // Validaciones de longitud
            if (!string.IsNullOrEmpty(representanteEmpresa) && representanteEmpresa.Length > 100)
                yield return CreateValidationResult("El representante de empresa no puede exceder 100 caracteres.", nameof(representanteEmpresa));

            if (!string.IsNullOrEmpty(representanteFacultad) && representanteFacultad.Length > 100)
                yield return CreateValidationResult("El representante de facultad no puede exceder 100 caracteres.", nameof(representanteFacultad));

            if (!string.IsNullOrEmpty(domicilioLegal) && domicilioLegal.Length > 255)
                yield return CreateValidationResult("El domicilio legal no puede exceder 255 caracteres.", nameof(domicilioLegal));

            if (!string.IsNullOrEmpty(domicilioAlternativo) && domicilioAlternativo.Length > 255)
                yield return CreateValidationResult("El domicilio alternativo no puede exceder 255 caracteres.", nameof(domicilioAlternativo));
        }

        public static IEnumerable<ValidationResult> ValidateConvenioEmpresaFiltro(
            string? nombreEmpresa, string? numeroAcuerdoMarco, bool? vigencia)
        {
            // No se requieren validaciones específicas para estos campos de búsqueda
            // Los campos son opcionales y se validan en el repositorio
            yield break;
        }

        public static IEnumerable<ValidationResult> ValidateAsignarEmpresa(
            int convenioId, int empresaId)
        {
            if (convenioId <= 0)
                yield return CreateValidationResult("El ID del convenio debe ser positivo.", nameof(convenioId));

            if (empresaId <= 0)
                yield return CreateValidationResult("El ID de la empresa debe ser positivo.", nameof(empresaId));
        }

        public static IEnumerable<ValidationResult> ValidateCaducarConvenio(
            string? fechaCaducidad)
        {
            if (IsNullOrWhiteSpace(fechaCaducidad))
                yield return CreateValidationResult("La fecha de caducidad es obligatoria.", nameof(fechaCaducidad));

            if (!DateTime.TryParse(fechaCaducidad, out var fecha))
                yield return CreateValidationResult("La fecha de caducidad debe tener un formato válido.", nameof(fechaCaducidad));
            else
            {
                if (fecha < DateTime.Now.Date)
                    yield return CreateValidationResult("La fecha de caducidad no puede ser anterior a hoy.", nameof(fechaCaducidad));
            }
        }
    }
}
