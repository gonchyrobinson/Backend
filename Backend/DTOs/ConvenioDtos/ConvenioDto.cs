namespace Backend.DTOs.ConvenioDtos
{
    public class ConvenioDto
    {
        public int IdConvenio { get; set; }
        public int? IdEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
        public int? NroAcuerdoMarco { get; set; }
        public string? DomicilioLegal { get; set; }
        public string? DomicilioAlternativo { get; set; }
        public string Expediente => $"EXP-FACET-{IdConvenio:D3}";
        public string? DocRepresentanteEmpresa { get; set; }
        public string? RepresentanteFacultad { get; set; }
        public string? DocRepresentanteFacultad { get; set; }
        public DateOnly? FechaFirma { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
        public string? Caracter { get; set; }
        public string? Sudocu { get; set; }
    }
}
