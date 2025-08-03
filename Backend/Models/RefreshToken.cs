using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class RefreshToken
{
    [Key]
    public string Token { get; set; } = string.Empty;
    
    public int UsuarioId { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsRevoked { get; set; }
    
    public DateTime? RevokedAt { get; set; }
    
    public string? RevokedBy { get; set; }
    
    public string? ReplacedByToken { get; set; }
    
    public string? ReasonRevoked { get; set; }
    
    public virtual Usuario Usuario { get; set; } = null!;
} 