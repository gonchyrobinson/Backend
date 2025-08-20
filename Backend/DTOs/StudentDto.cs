using System.ComponentModel.DataAnnotations;
public class StudentBusquedaAvanzadaDto : IValidatableObject
{
    public string? Documento { get; set; }
    public string? Carrera { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return StudentValidation.Validar(Carrera, Documento);
    }
}

public static class StudentValidation
{
    public static readonly HashSet<string> CarrerasValidas = new HashSet<string>
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

    public static IEnumerable<ValidationResult> Validar(string? carrera, string? documento, string? nombreCampoCarrera = "Carrera", string? nombreCampoDocumento = "Documento")
    {
        if (!string.IsNullOrEmpty(carrera) && !CarrerasValidas.Contains(carrera))
        {
            yield return new ValidationResult($"{nombreCampoCarrera} debe ser uno de los valores permitidos.", new[] { nombreCampoCarrera ?? string.Empty });
        }
        if (!string.IsNullOrEmpty(documento) && !EsDniValido(documento))
        {
            yield return new ValidationResult($"{nombreCampoDocumento} debe ser un DNI válido (solo números, 7 u 8 dígitos).", new[] { nombreCampoDocumento ?? string.Empty });
        }
    }

    public static bool EsDniValido(string documento)
    {
        return documento.All(char.IsDigit) && (documento.Length == 7 || documento.Length == 8);
    }
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
        public string? Carrera { get; set; }
        public string? AreaTrabajo { get; set; }
        public string? Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return StudentValidation.Validar(Carrera, Documento);
        }
    }
    public class StudentCreateDto : IValidatableObject
    {
        public string? Apellido { get; set; }
        public string? Nombre { get; set; }
        public string? Documento { get; set; }
        public string? Domicilio { get; set; }
        public string? Carrera { get; set; }
        public string? AreaTrabajo { get; set; }
        public string? Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return StudentValidation.Validar(Carrera, Documento);
        }
    }
}