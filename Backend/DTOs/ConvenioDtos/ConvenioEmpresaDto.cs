namespace Backend.DTOs.ConvenioDtos
{
    public class ConvenioEmpresaDto
    {
        public int IdConvenio { get; set; }
        public string NumeroConvenio => IdConvenio.ToString();
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
        public int? IdEmpresa { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
        public int? NroAcuerdoMarco { get; set; }
        public string? DomicilioLegal { get; set; }
        public string? DocumentoDecano { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? ExpedienteSudocu { get; set; }
    }
}
