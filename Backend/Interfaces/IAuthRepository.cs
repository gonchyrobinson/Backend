using Backend.DTOs;
using Backend.Models;

namespace Backend.Interfaces;

public interface IAuthRepository
{
    Task<Usuario?> GetUserByUsernameAsync(string username);
    Task<Usuario?> GetUserByEmailAsync(string email);
    Task<Usuario?> GetUserByIdAsync(int userId);
    Task<Usuario> CreateUserAsync(Usuario user);
    Task<bool> UserExistsAsync(string username, string email);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
} 