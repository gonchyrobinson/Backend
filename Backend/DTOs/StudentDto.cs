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
    public class StudentDto
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
    }

    public class StudentCreateDto
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
    }

    public class StudentUpdateDto
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
    }
}