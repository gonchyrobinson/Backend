using Backend.DTOs.AuthDtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/v1/authn")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario
    /// </summary>
    [HttpPost("register")]
    [Authorize]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);

            if (response == null)
            {
                return BadRequest(new { message = "Error al registrar usuario" });
            }

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Inicia sesión con credenciales de usuario
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);

            if (response == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene información de la sesión actual
    /// </summary>
    [HttpGet("session")]
    [Authorize]
    [ProducesResponseType(typeof(UserInfoDto), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetSession()
    {
        try
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var userInfo = await _authService.GetUserInfoAsync(userId);

            if (userInfo == null)
            {
                return Unauthorized(new { message = "Usuario no encontrado" });
            }

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene información de un usuario específico por ID
    /// </summary>
    [HttpGet("user/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(UserInfoDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var userInfo = await _authService.GetUserInfoAsync(id);

            if (userInfo == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza la información de un usuario
    /// </summary>
    [HttpPut("user")]
    [Authorize]
    [ProducesResponseType(typeof(UserInfoDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDto request)
    {
        try
        {
            var updatedUser = await _authService.UpdateUserAsync(request);

            if (updatedUser == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(updatedUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }
}