namespace Backend.DTOs.ConvenioDtos
{
    public class ConvenioEmpresaDto
    {
        public int IdConvenio { get; set; }
        public string Expediente => $"EXP-FACET-{IdConvenio:D3}";
        public DateOnly? FechaFirma { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
        public int? IdEmpresa { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
        public string? DomicilioLegal { get; set; }
        public string? DomicilioAlternativo { get; set; }
        public string? DocRepresentanteFacultad { get; set; }
        public string? Caracter { get; set; }
        public string? Sudocu { get; set; }
    }
}
