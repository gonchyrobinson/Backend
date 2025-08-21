using Backend.DTOs.AuditoriaDtos;
using Backend.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IServicioAuditoria _servicio;

        public AuditoriaController(IServicioAuditoria servicio)
        {
            _servicio = servicio;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<AuditoriaDto>>> Buscar([FromBody] AuditoriaBuscarDto buscarDto)
        {
            var logs = await _servicio.BuscarAsync(buscarDto);
            return Ok(logs);
        }
    }
}
