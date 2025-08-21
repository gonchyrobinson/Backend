using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public class PasantiaDtoValidator : BaseValidator
    {
        public static IEnumerable<ValidationResult> ValidatePasantia(
            int? idEstudiante, int? idConvenio, DateOnly? fechaInicio, DateOnly? fechaFin,
            string? tutorEmpresa, string? tutorFacultad, string? dniTutorFacultad,
            decimal? asignacionMensual, string? obraSocial, string? art,
            string? tipoAcuerdo, string? frecuenciaPago, int? horasSemanales,
            bool isCreate = false)
        {
            // Validaciones requeridas
            if (!idEstudiante.HasValue || idEstudiante <= 0)
                yield return CreateValidationResult("El ID del estudiante es obligatorio.", nameof(idEstudiante));

            if (!idConvenio.HasValue || idConvenio <= 0)
                yield return CreateValidationResult("El ID del convenio es obligatorio.", nameof(idConvenio));

            if (!fechaInicio.HasValue)
                yield return CreateValidationResult("La fecha de inicio es obligatoria.", nameof(fechaInicio));

            if (!fechaFin.HasValue)
                yield return CreateValidationResult("La fecha de fin es obligatoria.", nameof(fechaFin));

            if (IsNullOrWhiteSpace(tutorEmpresa))
                yield return CreateValidationResult("El tutor de empresa es obligatorio.", nameof(tutorEmpresa));

            if (IsNullOrWhiteSpace(tutorFacultad))
                yield return CreateValidationResult("El tutor de facultad es obligatorio.", nameof(tutorFacultad));

            if (IsNullOrWhiteSpace(dniTutorFacultad))
                yield return CreateValidationResult("El DNI del tutor de facultad es obligatorio.", nameof(dniTutorFacultad));

            if (IsNullOrWhiteSpace(obraSocial))
                yield return CreateValidationResult("La obra social es obligatoria.", nameof(obraSocial));

            if (IsNullOrWhiteSpace(art))
                yield return CreateValidationResult("La ART es obligatoria.", nameof(art));

            // Validaciones de formato
            if (!string.IsNullOrEmpty(dniTutorFacultad) && !CommonValidations.EsDniValido(dniTutorFacultad))
                yield return CreateValidationResult("El DNI del tutor de facultad debe ser válido.", nameof(dniTutorFacultad));

            if (!CommonValidations.EsTipoAcuerdoValido(tipoAcuerdo))
                yield return CreateValidationResult("El tipo de acuerdo debe ser válido (Pasantia, PPS, otro).", nameof(tipoAcuerdo));

            if (!CommonValidations.EsFrecuenciaPagoValida(frecuenciaPago))
                yield return CreateValidationResult("La frecuencia de pago debe ser válida (Mensual, Trimestral, Semestral, Anual).", nameof(frecuenciaPago));

            // Validaciones de fechas
            if (!CommonValidations.EsRangoFechasValido(fechaInicio, fechaFin))
                yield return CreateValidationResult("La fecha de inicio debe ser anterior a la fecha de fin.", nameof(fechaInicio));

            // Validaciones específicas por tipo de acuerdo
            if (tipoAcuerdo == "Pasantia")
            {
                if (!asignacionMensual.HasValue || asignacionMensual <= 0)
                    yield return CreateValidationResult("Las pasantías deben ser remuneradas.", nameof(asignacionMensual));

                // Validar duración mínima y máxima para pasantías
                if (fechaInicio.HasValue && fechaFin.HasValue)
                {
                    var meses = (fechaFin.Value.ToDateTime(TimeOnly.MinValue) - fechaInicio.Value.ToDateTime(TimeOnly.MinValue)).Days / 30.0;
                    if (meses < 2)
                        yield return CreateValidationResult("Las pasantías deben tener una duración mínima de 2 meses.", nameof(fechaFin));
                    if (meses > 12)
                        yield return CreateValidationResult("Las pasantías no pueden exceder 12 meses.", nameof(fechaFin));
                }
            }
            else if (tipoAcuerdo == "PPS")
            {
                if (asignacionMensual.HasValue && asignacionMensual > 0)
                    yield return CreateValidationResult("Las PPS no pueden ser remuneradas.", nameof(asignacionMensual));

                if (fechaInicio.HasValue && fechaFin.HasValue)
                {
                    var meses = (fechaFin.Value.ToDateTime(TimeOnly.MinValue) - fechaInicio.Value.ToDateTime(TimeOnly.MinValue)).Days / 30.0;
                    if (meses > 4)
                        yield return CreateValidationResult("Las PPS no pueden exceder 4 meses.", nameof(fechaFin));
                }
            }

            // Validaciones de horas semanales
            if (horasSemanales.HasValue)
            {
                if (horasSemanales <= 0)
                    yield return CreateValidationResult("Las horas semanales deben ser positivas.", nameof(horasSemanales));
                if (horasSemanales > 20)
                    yield return CreateValidationResult("Las horas semanales no pueden exceder 20.", nameof(horasSemanales));
            }

            // Validaciones de longitud
            if (!string.IsNullOrEmpty(tutorEmpresa) && tutorEmpresa.Length > 100)
                yield return CreateValidationResult("El tutor de empresa no puede exceder 100 caracteres.", nameof(tutorEmpresa));

            if (!string.IsNullOrEmpty(tutorFacultad) && tutorFacultad.Length > 100)
                yield return CreateValidationResult("El tutor de facultad no puede exceder 100 caracteres.", nameof(tutorFacultad));
        }

        public static IEnumerable<ValidationResult> ValidatePasantiaBusqueda(
            string? tramite, string? obraSocial, string? art, string? tutorEmpresa,
            string? tutorFacultad, string? tipoAcuerdo, DateOnly? fechaInicioDesde,
            DateOnly? fechaInicioHasta, DateOnly? fechaFinDesde, DateOnly? fechaFinHasta,
            int? idEstudiante, int? idConvenio, string? estado)
        {
            // Validar rangos de fechas
            if (fechaInicioDesde.HasValue && fechaInicioHasta.HasValue && fechaInicioDesde.Value > fechaInicioHasta.Value)
                yield return CreateValidationResult("FechaInicioDesde debe ser menor o igual a FechaInicioHasta.", nameof(fechaInicioDesde));

            if (fechaFinDesde.HasValue && fechaFinHasta.HasValue && fechaFinDesde.Value > fechaFinHasta.Value)
                yield return CreateValidationResult("FechaFinDesde debe ser menor o igual a FechaFinHasta.", nameof(fechaFinDesde));

            // Validar tipo de acuerdo si se proporciona
            if (!string.IsNullOrEmpty(tipoAcuerdo) && !CommonValidations.EsTipoAcuerdoValido(tipoAcuerdo))
                yield return CreateValidationResult("El tipo de acuerdo debe ser válido.", nameof(tipoAcuerdo));

            // Validar IDs si se proporcionan
            if (idEstudiante.HasValue && idEstudiante <= 0)
                yield return CreateValidationResult("El ID del estudiante debe ser positivo.", nameof(idEstudiante));

            if (idConvenio.HasValue && idConvenio <= 0)
                yield return CreateValidationResult("El ID del convenio debe ser positivo.", nameof(idConvenio));
        }
    }
}
