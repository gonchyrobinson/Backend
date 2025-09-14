using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class ConvenioDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidateConvenio(
            int? idEmpresa, string? representanteEmpresa, string? docRepresentanteEmpresa,
            string? nombreDecano, string? documentoDecano,
            DateOnly? fechaInicio, DateOnly? fechaCaducidad, string? domicilioLegal,
            string? tipoAcuerdo = null, bool isCreate = false)
        {
            // Validaciones requeridas
            if (!idEmpresa.HasValue || idEmpresa <= 0)
                yield return CreateValidationResult("El ID de la empresa es obligatorio.", nameof(idEmpresa));

            if (IsNullOrWhiteSpace(representanteEmpresa))
                yield return CreateValidationResult("El representante de empresa es obligatorio.", nameof(representanteEmpresa));

            if (IsNullOrWhiteSpace(docRepresentanteEmpresa))
                yield return CreateValidationResult("El documento del representante de empresa es obligatorio.", nameof(docRepresentanteEmpresa));

            if (IsNullOrWhiteSpace(nombreDecano))
                yield return CreateValidationResult("El nombre del decano es obligatorio.", nameof(nombreDecano));

            if (IsNullOrWhiteSpace(documentoDecano))
                yield return CreateValidationResult("El documento del decano es obligatorio.", nameof(documentoDecano));

            if (!fechaInicio.HasValue)
                yield return CreateValidationResult("La fecha de inicio es obligatoria.", nameof(fechaInicio));

            if (IsNullOrWhiteSpace(domicilioLegal))
                yield return CreateValidationResult("El domicilio legal es obligatorio.", nameof(domicilioLegal));

            // Validaciones de formato
            if (!string.IsNullOrEmpty(docRepresentanteEmpresa) && !CommonValidations.EsDniValido(docRepresentanteEmpresa))
                yield return CreateValidationResult("El documento del representante de empresa debe ser un DNI válido.", nameof(docRepresentanteEmpresa));

            if (!string.IsNullOrEmpty(documentoDecano) && !CommonValidations.EsDniValido(documentoDecano))
                yield return CreateValidationResult("El documento del decano debe ser un DNI válido.", nameof(documentoDecano));

            // Validaciones de fechas
            if (fechaInicio.HasValue && fechaInicio < DateOnly.FromDateTime(DateTime.Now.AddYears(-10)))
                yield return CreateValidationResult("La fecha de inicio no puede ser anterior a 10 años.", nameof(fechaInicio));

            if (fechaInicio.HasValue && fechaInicio > DateOnly.FromDateTime(DateTime.Now.AddDays(30)))
                yield return CreateValidationResult("La fecha de inicio no puede ser más de 30 días en el futuro.", nameof(fechaInicio));

            if (fechaCaducidad.HasValue && fechaInicio.HasValue && fechaCaducidad <= fechaInicio)
                yield return CreateValidationResult("La fecha de caducidad debe ser posterior a la fecha de inicio.", nameof(fechaCaducidad));

            // Validaciones de longitud
            if (!string.IsNullOrEmpty(representanteEmpresa) && representanteEmpresa.Length > 100)
                yield return CreateValidationResult("El representante de empresa no puede exceder 100 caracteres.", nameof(representanteEmpresa));

            if (!string.IsNullOrEmpty(nombreDecano) && nombreDecano.Length > 100)
                yield return CreateValidationResult("El nombre del decano no puede exceder 100 caracteres.", nameof(nombreDecano));

            if (!string.IsNullOrEmpty(domicilioLegal) && domicilioLegal.Length > 255)
                yield return CreateValidationResult("El domicilio legal no puede exceder 255 caracteres.", nameof(domicilioLegal));

            // Validación de TipoAcuerdo (solo validamos si viene un valor; valores históricos distintos permanecen)
            if (!string.IsNullOrWhiteSpace(tipoAcuerdo))
            {
                if (!Backend.Constants.AppConstants.ConvenioTipoAcuerdoPermitidos.Contains(tipoAcuerdo))
                    yield return CreateValidationResult(
                        $"TipoAcuerdo debe ser uno de: {string.Join(", ", Backend.Constants.AppConstants.ConvenioTipoAcuerdoPermitidos)}",
                        nameof(tipoAcuerdo));
            }

            // domicilioAlternativo eliminado del modelo/DTOs; validación removida
        }

        public static IEnumerable<ValidationResult> ValidateConvenioEmpresaFiltro(
            string? nombreEmpresa, string? expedienteSudocu, bool? vigencia)
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
