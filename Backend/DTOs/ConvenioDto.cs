namespace Backend.DTOs
{
    // DTO para listar convenios junto a empresa
    public class ConvenioEmpresaDto
    {
        public int IdConvenio { get; set; }
        public string? Expediente { get; set; }
        public DateOnly? FechaFirma { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
        public int? IdEmpresa { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
    }

    // DTO para asignar empresa a convenio
    public class AsignarEmpresaDto
    {
        public int ConvenioId { get; set; }
        public int EmpresaId { get; set; }
    }

    // DTO para caducar convenio
    public class CaducarConvenioDto
    {
        public int ConvenioId { get; set; }
        public DateOnly FechaCaducidad { get; set; }
    }

    public class ConvenioDto
    {
        public int IdConvenio { get; set; }
        public int? IdEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
        public int? NroAcuerdoMarco { get; set; }
        public string? DomicilioLegal { get; set; }
        public string? Expediente { get; set; }
        public string? DocRepresentanteEmpresa { get; set; }
        public string? RepresentanteFacultad { get; set; }
        public string? DocRepresentanteFacultad { get; set; }
        public DateOnly? FechaFirma { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
    }

    public class ConvenioCreateDto
    {
        public int? IdEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
        public int? NroAcuerdoMarco { get; set; }
        public string? DomicilioLegal { get; set; }
        public string? Expediente { get; set; }
        public string? DocRepresentanteEmpresa { get; set; }
        public string? RepresentanteFacultad { get; set; }
        public string? DocRepresentanteFacultad { get; set; }
        public DateOnly? FechaFirma { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
    }
}
