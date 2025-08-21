using Backend.DTOs.AuthDtos;
using Backend.Helpers;
using Backend.Interfaces.Repositories;
using Backend.DTOs.AuditoriaDtos;

using Backend.Models;
using Backend.Interfaces.Services;

namespace Backend.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto request);
    Task<UserInfoDto?> GetUserInfoAsync(int userId);
    Task<bool> ValidateCredentialsAsync(string username, string password);
}

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtService _jwtService;
    private readonly IServicioAuditoria _servicioAuditoria;

    public AuthService(IAuthRepository authRepository, IJwtService jwtService, IServicioAuditoria servicioAuditoria)
    {
        _authRepository = authRepository;
        _jwtService = jwtService;
        _servicioAuditoria = servicioAuditoria;
    }

    public async Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            // Verificar si el usuario ya existe
            var userExists = await _authRepository.UserExistsAsync(request.Username, request.Email);
            if (userExists)
            {
                var existingUserByUsername = await _authRepository.GetUserByUsernameAsync(request.Username);
                var existingUserByEmail = await _authRepository.GetUserByEmailAsync(request.Email);

                if (existingUserByUsername != null)
                    throw new InvalidOperationException("El nombre de usuario ya está en uso");
                else if (existingUserByEmail != null)
                    throw new InvalidOperationException("El email ya está registrado");
            }

            // Crear nuevo usuario
            var newUser = new Usuario
            {
                NombreUsuario = request.Username,
                Correo = request.Email,
                ContrasenaHash = PasswordHelper.HashPassword(request.Password),
                Rol = "admin", // Rol por defecto según ENUM de la DB
                Eliminado = false
            };

            var createdUser = await _authRepository.CreateUserAsync(newUser);

            // Generar token y retornar respuesta
            var token = _jwtService.GenerateAccessToken(createdUser);

            return new LoginResponseDto
            {
                Token = token,
                User = new UserInfoDto
                {
                    Id = createdUser.IdUsuario,
                    Username = createdUser.NombreUsuario ?? "",
                    Email = createdUser.Correo ?? "",
                    Role = createdUser.Rol ?? "admin"
                }
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al registrar usuario: {ex.Message}");
        }
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _authRepository.GetUserByUsernameAsync(request.Username);


        // Auditar intento de login fallido
        if (usuario == null || usuario.ContrasenaHash == null || !PasswordHelper.VerifyPassword(request.Password, usuario.ContrasenaHash))
        {
            await AuditarLoginAsync(null, request.Username, false);
            return null;
        }

        // Auditar login exitoso
        await AuditarLoginAsync(usuario.IdUsuario, usuario.NombreUsuario, true);

        var token = _jwtService.GenerateAccessToken(usuario);

        return new LoginResponseDto
        {
            Token = token,
            User = new UserInfoDto
            {
                Id = usuario.IdUsuario,
                Username = usuario.NombreUsuario ?? "",
                Email = usuario.Correo ?? "",
                Role = usuario.Rol ?? ""
            }
        };
    }

    public async Task<UserInfoDto?> GetUserInfoAsync(int userId)
    {
        var usuario = await _authRepository.GetUserByIdAsync(userId);

        if (usuario == null)
            return null;

        return new UserInfoDto
        {
            Id = usuario.IdUsuario,
            Username = usuario.NombreUsuario ?? "",
            Email = usuario.Correo ?? "",
            Role = usuario.Rol ?? ""
        };
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        return await _authRepository.ValidateUserCredentialsAsync(username, password);
    }

    private async Task AuditarLoginAsync(int? idUsuario, string? usuarioNombre, bool exito)
    {
        await _servicioAuditoria.RegistrarAsync(new AuditoriaDto
        {
            IdUsuario = idUsuario,
            UsuarioNombre = usuarioNombre,
            TablaAfectada = "USUARIOS",
            TipoOperacion = "LOGIN",
            DatosAnteriores = usuarioNombre,
            DatosNuevos = exito ? "logueado" : "sin loguear",
            FechaOperacion = DateTime.UtcNow,
            FuncionLlamada = "LoginAsync"
        });
    }
}