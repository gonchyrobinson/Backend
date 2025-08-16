using System.Collections.Generic;

namespace Backend.Reports.Contrato_Pasantia_Estudiante
{
    public class ContratoPasantiaEstudianteReportRenderer : IReporteRenderer<ReportData>
    {
        public Dictionary<string, string> MapFields(ReportData data)
        {
            return new Dictionary<string, string>
            {
                ["ConvenioRepresentanteEmpresa"] = data.ConvenioRepresentanteEmpresa,
                ["ConvenioDNIRepresentante"] = data.ConvenioDNIRepresentante,
                ["ConvenioRepresentanteFacultad"] = data.ConvenioRepresentanteFacultad,
                ["ConvenioDNIRepresentanteFacultad"] = data.ConvenioDNIRepresentanteFacultad,
                ["ConvenioDomicilioLegal"] = data.ConvenioDomicilioLegal,
                ["ConvenioExpediente"] = data.ConvenioExpediente,
                ["AsignacionMensual"] = data.AsignacionMensual,
                ["ObraSocial"] = data.ObraSocial,
                ["Art"] = data.Art,
                ["TutorEmpresa"] = data.TutorEmpresa,
                ["TutorFacultad"] = data.TutorFacultad,
                ["Expediente"] = data.ConvenioExpediente,
                ["MontoPago"] = data.MontoPago,
                ["Observaciones"] = data.Observaciones,
                ["TipoAcuerdo"] = data.TipoAcuerdo,
                ["EstudianteApellido"] = data.EstudianteApellido,
                ["EstudianteNombre"] = data.EstudianteNombre,
                ["EstudianteDNI"] = data.EstudianteDNI,
                ["EmpresaNombre"] = data.EmpresaNombre,
                ["EstudianteCarrera"] = data.EstudianteCarrera,
                ["EstudianteEmail"] = data.EstudianteEmail,
                ["EstudianteAreaDeTrabajo"] = data.EstudianteAreaDeTrabajo,
                ["EstudianteLibretaUniversitaria"] = data.EstudianteLibretaUniversitaria,
                ["EmpresaEncargado"] = data.EmpresaEncargado,
                ["EmpresaCelular"] = data.EmpresaCelular,
                ["EmpresaCorreoElectronico"] = data.EmpresaCorreoElectronico,
                ["EmpresaTipoContrato"] = data.EmpresaTipoContrato,
                ["EstudianteDomicilio"] = data.EstudianteDomicilio,
                ["FechaInicio"] = data.FechaInicio,
                ["FechaFin"] = data.FechaFin
            };
        }
    }
}
