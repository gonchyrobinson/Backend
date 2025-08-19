using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public partial class Pasantia
{
    public int IdPasantia { get; set; }

    [NotMapped]
    public string Tramite => $"TRA-FACET-{IdPasantia:D3}";

    public int? IdEstudiante { get; set; }

    public int? IdConvenio { get; set; }

    public decimal? AsignacionMensual { get; set; }

    public string? ObraSocial { get; set; }

    public string? Art { get; set; }

    public string? TutorEmpresa { get; set; }

    public string? TutorFacultad { get; set; }
    public string? DniTutorFacultad { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public string? TipoAcuerdo { get; set; }

    public string? Observaciones { get; set; }

    [Column("frecuencia_pago")]
    public string? FrecuenciaPago { get; set; }
    [Column("monto_pago")]

    public decimal? MontoPago { get; set; }
    public string? Sudocu { get; set; }
    public int? HorasSemanales { get; set; }

    public virtual Convenio? IdConvenioNavigation { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
