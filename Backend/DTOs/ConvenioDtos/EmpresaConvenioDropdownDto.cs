namespace Backend.DTOs.ConvenioDtos
{
    public class EmpresaConvenioDropdownDto
    {
        public int IdConvenio { get; set; }
        public int IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }
        public string Label => NombreEmpresa;
        public int Value => IdConvenio;
    }
}
