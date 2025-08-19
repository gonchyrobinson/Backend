using System.ComponentModel.DataAnnotations;
public class StudentBusquedaAvanzadaDto
{
    public string? Apellido { get; set; }
    public string? Nombre { get; set; }
    public string? Documento { get; set; }
    public string? Carrera { get; set; }
    public string? AreaTrabajo { get; set; }
}
namespace Backend.DTOs
{
    public class StudentDto : IValidatableObject
    {
        public int IdEstudiante { get; set; }
        public string? Apellido { get; set; }
        public string? Nombre { get; set; }
        public string? Documento { get; set; }
        public string? Domicilio { get; set; }
        public string? Libreta { get; set; }
        public string? Carrera { get; set; }
        public string? AreaTrabajo { get; set; }
        public string? Email { get; set; }
        private static readonly HashSet<string> CarrerasValidas = new HashSet<string>
        {
            "AGRIMENSURA",
            "INGENIERÍA AZUCARERA",
            "INGENIERÍA BIOMÉDICA",
            "INGENIERÍA CIVIL",
            "INGENIERÍA EN COMPUTACIÓN",
            "INGENIERÍA EN INFORMÁTICA",
            "INGENIERÍA ELÉCTRICA",
            "INGENIERÍA ELECTRÓNICA",
            "INGENIERÍA GEODÉSICA Y GEOFÍSICA",
            "INGENIERÍA INDUSTRIAL",
            "INGENIERÍA MECÁNICA",
            "INGENIERÍA QUÍMICA",
            "LICENCIATURA EN FÍSICA",
            "LICENCIATURA EN MATEMÁTICA",
            "LICENCIATURA EN INFORMÁTICA",
            "DISEÑO DE ILUMINACIÓN",
            "PROGRAMADOR UNIVERSITARIO",
            "TECNICATURA UNIVERSITARIA EN TECNOLOGÍA",
            "AZUCARERA E INDUSTRIAS DERIVADAS",
            "TECNICATURA UNIVERSITARIA EN FÍSICA",
            "TECNICATURA UNIVERSITARIA EN FÍSICA AMBIENTAL",
            "OTRA"
        };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Carrera) && !CarrerasValidas.Contains(Carrera))
            {
                yield return new ValidationResult($"Carrera debe ser uno de los valores permitidos.", new[] { nameof(Carrera) });
            }
        }
    }
    public class StudentCreateDto : IValidatableObject
    {
        private static readonly HashSet<string> CarrerasValidas = new HashSet<string>
        {
            "AGRIMENSURA",
            "INGENIERÍA AZUCARERA",
            "INGENIERÍA BIOMÉDICA",
            "INGENIERÍA CIVIL",
            "INGENIERÍA EN COMPUTACIÓN",
            "INGENIERÍA EN INFORMÁTICA",
            "INGENIERÍA ELÉCTRICA",
            "INGENIERÍA ELECTRÓNICA",
            "INGENIERÍA GEODÉSICA Y GEOFÍSICA",
            "INGENIERÍA INDUSTRIAL",
            "INGENIERÍA MECÁNICA",
            "INGENIERÍA QUÍMICA",
            "LICENCIATURA EN FÍSICA",
            "LICENCIATURA EN MATEMÁTICA",
            "LICENCIATURA EN INFORMÁTICA",
            "DISEÑO DE ILUMINACIÓN",
            "PROGRAMADOR UNIVERSITARIO",
            "TECNICATURA UNIVERSITARIA EN TECNOLOGÍA",
            "AZUCARERA E INDUSTRIAS DERIVADAS",
            "TECNICATURA UNIVERSITARIA EN FÍSICA",
            "TECNICATURA UNIVERSITARIA EN FÍSICA AMBIENTAL",
            "OTRA"
        };

        public string? Apellido { get; set; }
        public string? Nombre { get; set; }
        public string? Documento { get; set; }
        public string? Domicilio { get; set; }
        public string? Libreta { get; set; }
        public string? Carrera { get; set; }
        public string? AreaTrabajo { get; set; }
        public string? Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Carrera) && !CarrerasValidas.Contains(Carrera))
            {
                yield return new ValidationResult($"Carrera debe ser uno de los valores permitidos.", new[] { nameof(Carrera) });
            }
        }
    }
}