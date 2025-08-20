namespace Backend.Reports.Contrato_Pasantia_Estudiante
{
    public class ContratoPasantiaEstudianteReportRenderer : IReporteRenderer<ReportData>
    {
        public Dictionary<string, string> MapFields(ReportData data)
        {
            return new Dictionary<string, string>
            {
                ["ConvenioRepresentanteEmpresa"] = data.ConvenioRepresentanteEmpresa ?? string.Empty,
                ["ConvenioDNIRepresentante"] = data.ConvenioDNIRepresentante ?? string.Empty,
                ["ConvenioRepresentanteFacultad"] = data.ConvenioRepresentanteFacultad ?? string.Empty,
                ["ConvenioDNIRepresentanteFacultad"] = data.ConvenioDNIRepresentanteFacultad ?? string.Empty,
                ["ConvenioDomicilioLegal"] = data.ConvenioDomicilioLegal ?? string.Empty,
                ["ConvenioExpediente"] = data.ConvenioExpediente ?? string.Empty,
                ["AsignacionMensual"] = data.AsignacionMensual ?? string.Empty,
                ["ObraSocial"] = data.ObraSocial ?? string.Empty,
                ["Art"] = data.Art ?? string.Empty,
                ["TutorEmpresa"] = data.TutorEmpresa ?? string.Empty,
                ["TutorFacultad"] = data.TutorFacultad ?? string.Empty,
                ["Observaciones"] = data.Observaciones ?? string.Empty,
                ["TipoAcuerdo"] = data.TipoAcuerdo ?? string.Empty,
                ["EstudianteApellido"] = data.EstudianteApellido ?? string.Empty,
                ["EstudianteNombre"] = data.EstudianteNombre ?? string.Empty,
                ["EstudianteDNI"] = data.EstudianteDNI ?? string.Empty,
                ["EmpresaNombre"] = data.EmpresaNombre ?? string.Empty,
                ["EstudianteCarrera"] = data.EstudianteCarrera ?? string.Empty,
                ["EstudianteEmail"] = data.EstudianteEmail ?? string.Empty,
                ["EstudianteAreaDeTrabajo"] = data.EstudianteAreaDeTrabajo ?? string.Empty,
                ["EmpresaEncargado"] = data.EmpresaEncargado ?? string.Empty,
                ["EmpresaCelular"] = data.EmpresaCelular ?? string.Empty,
                ["EmpresaCorreoElectronico"] = data.EmpresaCorreoElectronico ?? string.Empty,
                ["EmpresaTipoContrato"] = data.EmpresaTipoContrato ?? string.Empty,
                ["EstudianteDomicilio"] = data.EstudianteDomicilio ?? string.Empty,
                ["FechaInicio"] = data.FechaInicio ?? string.Empty,
                ["FechaFin"] = data.FechaFin ?? string.Empty
            };
        }
    }
}
