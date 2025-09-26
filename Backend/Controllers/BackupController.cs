using Backend.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BackupController : ControllerBase
    {
        private readonly IBackupService _backupService;

        public BackupController(IBackupService backupService)
        {
            _backupService = backupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBackup()
        {
            try
            {
                var backupData = await _backupService.GenerateBackupAsync();
                var fileName = $"backup_pasantias_db_{DateTime.UtcNow:yyyyMMdd_HHmmss}.sql";
                
                return File(backupData, "application/sql", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating backup", error = ex.Message });
            }
        }
    }
}