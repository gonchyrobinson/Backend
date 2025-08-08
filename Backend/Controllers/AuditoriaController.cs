using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly ServicioAuditoria _servicio;

        public AuditoriaController(ServicioAuditoria servicio)
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
