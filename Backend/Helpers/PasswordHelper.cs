using System.Security.Cryptography;
using System.Text;

namespace Backend.Helpers;

public static class PasswordHelper
{
    /// <summary>
    /// Genera un hash SHA256 de la contraseña
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <returns>Hash de la contraseña</returns>
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verifica si una contraseña coincide con su hash
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <param name="hash">Hash de la contraseña</param>
    /// <returns>True si la contraseña coincide</returns>
    public static bool VerifyPassword(string password, string hash)
    {
        var computedHash = HashPassword(password);
        return hash.Equals(computedHash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Genera una contraseña aleatoria
    /// </summary>
    /// <param name="length">Longitud de la contraseña</param>
    /// <returns>Contraseña aleatoria</returns>
    public static string GenerateRandomPassword(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}