namespace Backend.Reports.ExtensionSeguro
{
    public class ExtensionSeguroReportRenderer : IReporteRenderer<ReportData>
    {
        public Dictionary<string, string> MapFields(ReportData data)
        {
            return new Dictionary<string, string>
            {
                ["diaHoy"] = DateTime.Now.Day.ToString(),
                ["MesHoy"] = DateTime.Now.ToString("MMMM"),
                ["AñoHoy"] = DateTime.Now.Year.ToString(),
                ["ConvenioRepresentanteFacultad"] = data.ConvenioRepresentanteFacultad ?? string.Empty,
                ["EstudianteCarrera"] = data.EstudianteCarrera ?? string.Empty,
                ["EstudianteNombre"] = data.EstudianteNombre ?? string.Empty,
                ["EstudianteDNI"] = data.EstudianteDNI ?? string.Empty,
                ["FechaInicio"] = data.FechaInicio ?? string.Empty,
                ["FechaFin"] = data.FechaFin ?? string.Empty,
                ["EmpresaNombre"] = data.EmpresaNombre ?? string.Empty,
                ["ConvenioDomicilioLegal"] = data.ConvenioDomicilioLegal ?? string.Empty,
                ["TutorFacultad"] = data.TutorFacultad ?? string.Empty,
                ["DNITutorFacultad"] = data.DniTutorFacultad ?? string.Empty,
                // Removed ConvenioDomicilioAlternativo
            };
        }
    }
}
