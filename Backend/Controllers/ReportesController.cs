using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.Reports;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ReportesController : ControllerBase
    {
        private readonly BasePdfReportService<object, object> _pdfReportService; // Reemplaza los genéricos por los concretos

        protected ReportesController(BasePdfReportService<object, object> pdfReportService)
        {
            _pdfReportService = pdfReportService;
        }

        protected abstract string FileName { get; }

        [HttpGet("descargar-pdf/{id}")]
        public virtual async Task<IActionResult> DescargarPdf(int id)
        {
            // El método espera que el service tenga un método GetPdfFileResultAsync(int id)
            var method = _pdfReportService.GetType().GetMethod("GetPdfFileResultAsync");
            if (method == null)
                return NotFound("Método GetPdfFileResultAsync no implementado en el service.");
            var taskObj = method.Invoke(_pdfReportService, new object[] { id });
            if (taskObj is not Task task)
                return NotFound("No se pudo generar el archivo PDF.");
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            var fileResult = resultProperty?.GetValue(task) as FileResult;
            if (fileResult == null)
                return NotFound("No se pudo generar el archivo PDF.");
            return fileResult;
        }
    }
}
