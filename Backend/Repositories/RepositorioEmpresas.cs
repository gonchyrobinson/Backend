using Backend.DTOs;
using Backend.Contexts;
using Backend.Models;
using Backend.Interfaces;

namespace Backend.Repositories
{
    public class RepositorioEmpresas : Repository<Empresa>, IRepositorioEmpresas
    {
        public RepositorioEmpresas(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Empresa>> BuscarAvanzadoAsync(EmpresaBusquedaAvanzadaDto filtro)
        {
            var empresas = await GetAllAsync();

            bool IsStringValid(string? s) => !string.IsNullOrWhiteSpace(s) && s != "string";
            bool IsDateValid(DateOnly? d) => d.HasValue && d.Value != DateOnly.MinValue && d.Value != DateOnly.FromDateTime(DateTime.Today);

            if (IsStringValid(filtro.Nombre))
                empresas = empresas.Where(e => !string.IsNullOrEmpty(e.Nombre) && e.Nombre.ToLower().Contains(filtro.Nombre!.ToLower()));

            if (IsStringValid(filtro.Vigencia))
                empresas = empresas.Where(e => !string.IsNullOrEmpty(e.Vigencia) && e.Vigencia.ToLower() == filtro.Vigencia!.ToLower());

            if (IsStringValid(filtro.TipoContrato))
                empresas = empresas.Where(e => !string.IsNullOrEmpty(e.TipoContrato) && e.TipoContrato.ToLower() == filtro.TipoContrato!.ToLower());

            if (IsDateValid(filtro.FechaInicioDesde))
                empresas = empresas.Where(e => e.FechaInicio >= filtro.FechaInicioDesde);

            if (IsDateValid(filtro.FechaInicioHasta))
                empresas = empresas.Where(e => e.FechaInicio <= filtro.FechaInicioHasta);

            if (IsDateValid(filtro.FechaFinDesde))
                empresas = empresas.Where(e => e.FechaFin >= filtro.FechaFinDesde);

            if (IsDateValid(filtro.FechaFinHasta))
                empresas = empresas.Where(e => e.FechaFin <= filtro.FechaFinHasta);

            return empresas;
        }
    }
}
