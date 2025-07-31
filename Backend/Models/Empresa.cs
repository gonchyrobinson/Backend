using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Empresa
{
    public int IdEmpresa { get; set; }

    public string? Nombre { get; set; }

    public string? Vigencia { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public string? TipoContrato { get; set; }

    public string? Encargado { get; set; }

    public string? Celular { get; set; }

    public string? CorreoElectronico { get; set; }

    public DateOnly? Sudocu { get; set; }

    public bool? Eliminado { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<Convenio> Convenios { get; set; } = new List<Convenio>();
}
