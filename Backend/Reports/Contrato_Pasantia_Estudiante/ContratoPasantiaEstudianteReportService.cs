using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Reports.Contrato_Pasantia_Estudiante
{
    public class ContratoPasantiaEstudianteReportService : BasePdfReportService<int, ContratoPasantiaEstudianteReportData>
    {
        private const string TemplateFileName = "contrato_pasantia_Estudiante.pdf";

        public ContratoPasantiaEstudianteReportService(
            IReporteDataAggregator<int, ContratoPasantiaEstudianteReportData> aggregator,
            IReporteRenderer<ContratoPasantiaEstudianteReportData> renderer)
            : base(aggregator, renderer, System.IO.Path.Combine("Reports", "Templates", TemplateFileName))
        {
        }

        public async Task<FileContentResult> GetPdfFileResultAsync(int id)
        {
            var pdfBytes = await GenerateReportAsync(id);
            return new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = TemplateFileName
            };
        }
    }
}
