using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    /// <summary>
    /// Controlador base genérico para reportes PDF.
    /// Hereda de ControllerBase y expone un método para descarga de PDF.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]

    public abstract class BaseReportController<TRequest, TService> : ControllerBase
        where TService : class
    {
        protected readonly TService _reportService;

        /// <summary>
        /// Nombre sugerido por defecto para el archivo PDF. Debe ser implementado en cada controlador concreto.
        /// </summary>
        protected abstract string FileName { get; }

        protected BaseReportController(TService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Endpoint genérico para descargar un PDF generado por el servicio de reportes.
        /// </summary>
        /// <param name="request">Parámetros del reporte</param>
        /// <param name="fileName">Nombre sugerido para el archivo</param>
        /// <returns>Archivo PDF listo para descarga</returns>
        [HttpGet("descargar-pdf/{id}")]
        public virtual async Task<IActionResult> DescargarPdf(int id)
        {
            // El método espera que el service tenga un método GetPdfFileResultAsync(int id)
            // Este método debe ser implementado en el service concreto
            var method = _reportService.GetType().GetMethod("GetPdfFileResultAsync");
            if (method == null)
                return NotFound("Método GetPdfFileResultAsync no implementado en el service.");
            var taskObj = method.Invoke(_reportService, new object[] { id });
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
