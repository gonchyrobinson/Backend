namespace Backend.DTOs
{
    // DTO para GET y UPDATE
    public class PagosDto
    {
        public int IdPago { get; set; }
        public int? IdPasantia { get; set; }
        public bool? Pagado { get; set; }
        public DateOnly? FechaPago { get; set; }
        public DateOnly? FechaVencimiento { get; set; }
        public decimal? Monto { get; set; }
        public string? Observaciones { get; set; }
    }

    // DTO para CREATE
    public class CreatePagosDto
    {
        public int? IdPasantia { get; set; }
        public DateOnly? FechaPago { get; set; }
        public DateOnly? FechaVencimiento { get; set; }
        public decimal? Monto { get; set; }
        public string? Observaciones { get; set; }
    }

    public class MarcarPagoDto
    {
        public int IdPago { get; set; }
        public DateOnly? FechaPago { get; set; }
    }
}
