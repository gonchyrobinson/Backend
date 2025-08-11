using Microsoft.AspNetCore.Mvc;

namespace Backend.Reports.Contrato_Pasantia_Estudiante
{
    [Route("api/reporte-contrato-pasantia-estudiante")]
    [ApiController]
    public class ContratoPasantiaEstudianteReportController : ControllerBase
    {
        private readonly ContratoPasantiaEstudianteReportService _service;

        public ContratoPasantiaEstudianteReportController(ContratoPasantiaEstudianteReportService service)
        {
            _service = service;
        }

        [HttpGet("descargar-pdf/{id}")]
        public async Task<IActionResult> DescargarPdf(int id)
        {
            var result = await _service.GetPdfFileResultAsync(id);
            return result;
        }
    }
}
