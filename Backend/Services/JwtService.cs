using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Services;

public interface IJwtService
{
    string GenerateAccessToken(Usuario usuario);
    ClaimsPrincipal? ValidateToken(string token);
    UserInfoDto GetUserInfoFromToken(string token);
}

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SymmetricSecurityKey CreateSigningKey()
{
    var secret = _configuration["Jwt:SecretKey"] 
                 ?? throw new InvalidOperationException("JWT SecretKey not configured");

    var keyBytes = Encoding.UTF8.GetBytes(secret);

    if (keyBytes.Length < 32)
        throw new InvalidOperationException("JWT key must be at least 256 bits (32 bytes).");

    return new SymmetricSecurityKey(keyBytes);
}

    public string GenerateAccessToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = CreateSigningKey();
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Name, usuario.NombreUsuario ?? ""),
            new(ClaimTypes.Email, usuario.Correo ?? ""),
            new(ClaimTypes.Role, usuario.Rol ?? ""),
            new("UserId", usuario.IdUsuario.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15")),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = CreateSigningKey();

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public UserInfoDto GetUserInfoFromToken(string token)
    {
        var principal = ValidateToken(token);
        if (principal == null)
            throw new InvalidOperationException("Invalid token");

        var userIdClaim = principal.FindFirst("UserId")?.Value;
        var nameClaim = principal.FindFirst(ClaimTypes.Name)?.Value;
        var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
        var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;

        return new UserInfoDto
        {
            Id = int.Parse(userIdClaim ?? "0"),
            Username = nameClaim ?? "",
            Email = emailClaim ?? "",
            Role = roleClaim ?? ""
        };
    }
} 