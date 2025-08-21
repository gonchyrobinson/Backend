using Backend.Contexts;
using Backend.Helpers;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetUserByUsernameAsync(string username)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreUsuario == username && u.Eliminado != true);
    }

    public async Task<Usuario?> GetUserByEmailAsync(string email)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Correo == email && u.Eliminado != true);
    }

    public async Task<Usuario?> GetUserByIdAsync(int userId)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdUsuario == userId && u.Eliminado != true);
    }

    public async Task<Usuario> CreateUserAsync(Usuario user)
    {
        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UserExistsAsync(string username, string email)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .AnyAsync(u => (u.NombreUsuario == username || u.Correo == email) && u.Eliminado != true);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        var user = await GetUserByUsernameAsync(username);
        if (user == null) return false;

        return user.ContrasenaHash != null && PasswordHelper.VerifyPassword(password, user.ContrasenaHash);
    }
}