using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? ContrasenaHash { get; set; }

    public string? Rol { get; set; }

    public string? Correo { get; set; }

    public bool? Eliminado { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<Auditoria> Auditoria { get; set; } = new List<Auditoria>();
}
