using Backend.Helpers;
using Backend.Models;

namespace Backend.Reports
{
    public static class ReportDataFactory
    {
        public static ReportData FromEntities(
            Pasantia pasantia,
            Estudiante estudiante,
            Empresa empresa,
            Convenio convenio)
        {
            string? SafeString(object? value)
            {
                if (value == null || value is System.DBNull) return null;
                return value.ToString();
            }

            string? SafeDecimal(object? value)
            {
                if (value == null || value is System.DBNull) return null;
                if (value is decimal d) return d.ToString("N2");
                if (decimal.TryParse(value.ToString(), out var result)) return result.ToString("N2");
                return null;
            }

            string? SafeDate(object? value)
            {
                if (value == null || value is System.DBNull) return null;
                if (value is DateOnly date) return date.ToString(DateFormats.DefaultDateFormat);
                if (DateOnly.TryParse(value.ToString(), out var result)) return result.ToString(DateFormats.DefaultDateFormat);
                return null;
            }

            return new ReportData
            {
                // Convenio
                ConvenioRepresentanteEmpresa = SafeString(convenio?.RepresentanteEmpresa),
                ConvenioDNIRepresentante = SafeString(convenio?.DocRepresentanteEmpresa),
                ConvenioRepresentanteFacultad = SafeString(convenio?.NombreDecano),
                ConvenioDNIRepresentanteFacultad = SafeString(convenio?.DocumentoDecano),
                ConvenioDomicilioLegal = SafeString(convenio?.DomicilioLegal),
                NumeroConvenio = convenio != null ? convenio.IdConvenio.ToString() : null,

                // Pasantía
                AsignacionMensual = SafeDecimal(pasantia?.AsignacionMensual),
                ObraSocial = SafeString(pasantia?.ObraSocial),
                Art = SafeString(pasantia?.Art),
                TutorEmpresa = SafeString(pasantia?.TutorEmpresa),
                Tramite = pasantia != null ? $"TRA-FACET-{pasantia.IdPasantia:D3}" : null,
                TutorFacultad = SafeString(pasantia?.TutorFacultad),
                DniTutorFacultad = SafeString(pasantia?.DniTutorFacultad),
                Observaciones = SafeString(pasantia?.Observaciones),
                FechaInicio = SafeDate(pasantia?.FechaInicio),
                FechaFin = SafeDate(pasantia?.FechaFin),
                TipoAcuerdo = SafeString(pasantia?.TipoAcuerdo),
                HorasSemanales = SafeString(pasantia?.HorasSemanales),

                // Estudiante
                EstudianteApellido = SafeString(estudiante?.Apellido),
                EstudianteNombre = SafeString(estudiante?.Nombre),
                EstudianteDNI = SafeString(estudiante?.Documento),
                EmpresaNombre = SafeString(empresa?.Nombre),
                EstudianteCarrera = SafeString(estudiante?.Carrera),
                EstudianteEmail = SafeString(estudiante?.Email),
                EmpresaEncargado = SafeString(empresa?.Encargado),
                EmpresaCelular = SafeString(empresa?.Celular),
                EmpresaCorreoElectronico = SafeString(empresa?.CorreoElectronico),
                EmpresaTipoContrato = SafeString(empresa?.TipoContrato),
                EstudianteDomicilio = SafeString(estudiante?.Domicilio)
            };
        }
    }
}
