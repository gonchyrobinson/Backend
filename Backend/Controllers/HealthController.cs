using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Health check requested at {Time}", DateTime.UtcNow);

            var healthStatus = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                MachineName = Environment.MachineName,
                ProcessId = Environment.ProcessId,
                MemoryUsage = GC.GetTotalMemory(false),
                Uptime = Process.GetCurrentProcess().TotalProcessorTime
            };

            return Ok(healthStatus);
        }

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            // Aquí puedes agregar verificaciones adicionales
            // como conexión a base de datos, servicios externos, etc.

            return Ok(new { Status = "Ready", Timestamp = DateTime.UtcNow });
        }

        [HttpGet("live")]
        public IActionResult Live()
        {
            return Ok(new { Status = "Alive", Timestamp = DateTime.UtcNow });
        }
    }
}