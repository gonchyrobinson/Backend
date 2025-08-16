namespace Backend.DTOs
{
    // DTO para filtros de búsqueda de convenios junto a empresa
    public class ConvenioEmpresaFiltroDto : System.ComponentModel.DataAnnotations.IValidatableObject
    {
        public DateOnly? FechaFirmaDesde { get; set; }
        public DateOnly? FechaFirmaHasta { get; set; }
        public DateOnly? FechaCaducidadDesde { get; set; }
        public DateOnly? FechaCaducidadHasta { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? DocRepresentanteFacultad { get; set; }
        public string? Carrera { get; set; }

        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            var carrerasValidas = new[] {
                "AGRIMENSURA", "INGENIERÍA AZUCARERA", "INGENIERÍA BIOMÉDICA", "INGENIERÍA CIVIL",
                "INGENIERÍA EN COMPUTACIÓN", "INGENIERÍA EN INFORMÁTICA", "INGENIERÍA ELÉCTRICA",
                "INGENIERÍA ELECTRÓNICA", "INGENIERÍA GEODÉSICA Y GEOFÍSICA", "INGENIERÍA INDUSTRIAL",
                "INGENIERÍA MECÁNICA", "INGENIERÍA QUÍMICA", "LICENCIATURA EN FÍSICA", "LICENCIATURA EN MATEMÁTICA",
                "LICENCIATURA EN INFORMÁTICA", "DISEÑO DE ILUMINACIÓN", "PROGRAMADOR UNIVERSITARIO",
                "TECNICATURA UNIVERSITARIA EN TECNOLOGÍA", "AZUCARERA E INDUSTRIAS DERIVADAS",
                "TECNICATURA UNIVERSITARIA EN FÍSICA", "TECNICATURA UNIVERSITARIA EN FÍSICA AMBIENTAL"
            };
            if (!string.IsNullOrEmpty(Carrera) && !carrerasValidas.Contains(Carrera))
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult($"Carrera debe ser una de las siguientes: {string.Join(", ", carrerasValidas)}.", new[] { nameof(Carrera) });
            }
        }
    }
    // DTO para listar convenios junto a empresa
    public class ConvenioEmpresaDto
    {
        public int IdConvenio { get; set; }
        public string Expediente => $"EXP-FACET-ID_{IdConvenio:D3}";
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

    public class ConvenioCreateDto
    {
        public int? IdEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
        public int? NroAcuerdoMarco { get; set; }
        public string? DomicilioLegal { get; set; }
        public string? DomicilioAlternativo { get; set; }
        public string? DocRepresentanteEmpresa { get; set; }
        public string? RepresentanteFacultad { get; set; }
        public string? DocRepresentanteFacultad { get; set; }
        public DateOnly? FechaFirma { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
        public string? Caracter { get; set; }
        public string? Sudocu { get; set; }
    }
}
