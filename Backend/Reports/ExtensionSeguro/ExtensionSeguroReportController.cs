using Microsoft.AspNetCore.Mvc;

namespace Backend.Reports.ExtensionSeguro
{
    [Route("api/reporte-extension-seguro")]
    [ApiController]
    public class ExtensionSeguroReportController : ControllerBase
    {
        private readonly ExtensionSeguroReportService _service;

        public ExtensionSeguroReportController(ExtensionSeguroReportService service)
        {
            _service = service;
        }

        [HttpGet("descargar-docx/{id}")]
        public async Task<IActionResult> DescargarDocx(int id)
        {
            var result = await _service.GetDocxFileResultAsync(id);
            return result;
        }
    }
}
