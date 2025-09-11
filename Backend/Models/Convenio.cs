namespace Backend.Models;

public partial class Convenio
{
    public int IdConvenio { get; set; }

    public int? IdEmpresa { get; set; }

    public string? RepresentanteEmpresa { get; set; }

    public int? NroAcuerdoMarco { get; set; }

    public string? DomicilioLegal { get; set; }


    public string? DocRepresentanteEmpresa { get; set; }

    public string? NombreDecano { get; set; }

    public string? DocumentoDecano { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaCaducidad { get; set; }

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual ICollection<Pasantia> Pasantia { get; set; } = new List<Pasantia>();
    public string? TipoAcuerdo { get; set; }
    public string? ExpedienteSudocu { get; set; }
}
