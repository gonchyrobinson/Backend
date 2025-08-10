using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public int? IdPasantia { get; set; }

    public bool? Pagado { get; set; }

    public DateOnly? FechaPago { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("fecha_vencimiento")]
    public DateOnly? FechaVencimiento { get; set; }

    public decimal? Monto { get; set; }

    public string? Observaciones { get; set; }

    public virtual Pasantia? IdPasantiaNavigation { get; set; }
}
