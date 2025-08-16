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
                if (value is DateOnly date) return date.ToString("yyyy-MM-dd");
                if (DateOnly.TryParse(value.ToString(), out var result)) return result.ToString("yyyy-MM-dd");
                return null;
            }

            return new ReportData
            {
                // Convenio
                ConvenioRepresentanteEmpresa = SafeString(convenio?.RepresentanteEmpresa),
                ConvenioDNIRepresentante = SafeString(convenio?.DocRepresentanteEmpresa),
                ConvenioRepresentanteFacultad = SafeString(convenio?.RepresentanteFacultad),
                ConvenioDNIRepresentanteFacultad = SafeString(convenio?.DocRepresentanteFacultad),
                ConvenioDomicilioLegal = SafeString(convenio?.DomicilioLegal),
                ConvenioExpediente = convenio != null ? $"TRA-FACET-{convenio.IdConvenio:D3}" : null,

                // Pasantía
                AsignacionMensual = SafeDecimal(pasantia?.AsignacionMensual),
                ObraSocial = SafeString(pasantia?.ObraSocial),
                Art = SafeString(pasantia?.Art),
                TutorEmpresa = SafeString(pasantia?.TutorEmpresa),
                Tramite = pasantia != null ? $"TRA-FACET-{pasantia.IdPasantia:D3}" : null,
                TutorFacultad = SafeString(pasantia?.TutorFacultad),
                DniTutorFacultad = SafeString(pasantia?.DniTutorFacultad),
                MontoPago = SafeDecimal(pasantia?.MontoPago),
                Observaciones = SafeString(pasantia?.Observaciones),
                FechaInicio = SafeDate(pasantia?.FechaInicio),
                FechaFin = SafeDate(pasantia?.FechaFin),
                TipoAcuerdo = SafeString(pasantia?.TipoAcuerdo),

                // Estudiante
                EstudianteApellido = SafeString(estudiante?.Apellido),
                EstudianteNombre = SafeString(estudiante?.Nombre),
                EstudianteDNI = SafeString(estudiante?.Documento),
                EmpresaNombre = SafeString(empresa?.Nombre),
                EstudianteCarrera = SafeString(estudiante?.Carrera),
                EstudianteEmail = SafeString(estudiante?.Email),
                EstudianteAreaDeTrabajo = SafeString(estudiante?.AreaTrabajo),
                EstudianteLibretaUniversitaria = SafeString(estudiante?.Libreta),
                EmpresaEncargado = SafeString(empresa?.Encargado),
                EmpresaCelular = SafeString(empresa?.Celular),
                EmpresaCorreoElectronico = SafeString(empresa?.CorreoElectronico),
                EmpresaTipoContrato = SafeString(empresa?.TipoContrato),
                EstudianteDomicilio = SafeString(estudiante?.Domicilio)
            };
        }
    }
}
