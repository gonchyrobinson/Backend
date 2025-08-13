using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Reports.Contrato_Pasantia_Estudiante;
using Backend.Reports;

namespace Backend.Reports.ExtensionSeguro
{
    public class ExtensionSeguroReportService : BaseDocxReportService<int, ReportData>
    {
        private const string TemplateFileName = "extension_seguro.docx";

        public ExtensionSeguroReportService(
            IReporteDataAggregator<int, ReportData> aggregator,
            IReporteRenderer<ReportData> renderer)
            : base(aggregator, renderer, System.IO.Path.Combine("Reports", "Templates", TemplateFileName))
        {
        }

        public async Task<FileContentResult> GetDocxFileResultAsync(int id)
        {
            var docxBytes = await GenerateReportAsync(id);
            return new FileContentResult(docxBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = TemplateFileName
            };
        }
    }
}
