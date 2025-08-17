namespace Backend.Models;

public partial class Auditoria

{
    public int IdAuditoria { get; set; }

    public int? IdUsuario { get; set; }

    public string? TablaAfectada { get; set; }

    public string? TipoOperacion { get; set; }

    public string? DatosAnteriores { get; set; }

    public string? DatosNuevos { get; set; }

    public DateTime? FechaOperacion { get; set; }

    public string? FuncionLlamada { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
