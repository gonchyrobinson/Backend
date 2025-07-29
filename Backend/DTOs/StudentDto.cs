namespace Backend.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Carrera { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }
}