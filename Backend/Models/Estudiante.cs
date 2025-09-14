namespace Backend.Models;

public partial class Estudiante
{
    public int IdEstudiante { get; set; }

    public string? Apellido { get; set; }

    public string? Nombre { get; set; }

    public string? Documento { get; set; }

    public string? Domicilio { get; set; }

    public string? Libreta { get; set; }

    public string? Carrera { get; set; }
    public string? Email { get; set; }

    public bool? Eliminado { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<Pasantia> Pasantia { get; set; } = new List<Pasantia>();
}
