using Backend.Contexts;
using Backend.DTOs.PagosDtos;
using Backend.Exceptions;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RepositorioPagos : Repository<Pago>, IRepositorioPagos
    {
        public RepositorioPagos(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Pago>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdEstudianteNavigation)
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdConvenioNavigation)
                .ThenInclude(c => c.IdEmpresaNavigation)
                .ToListAsync();
        }

        public override async Task<Pago?> GetByIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdEstudianteNavigation)
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdConvenioNavigation)
                .ThenInclude(c => c.IdEmpresaNavigation)
                .FirstOrDefaultAsync(p => p.IdPago == id);
        }

        public async Task<IEnumerable<Pago>> GetByPasantiaIdAsync(int idPasantia)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdEstudianteNavigation)
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdConvenioNavigation)
                .ThenInclude(c => c.IdEmpresaNavigation)
                .Where(p => p.IdPasantia == idPasantia)
                .ToListAsync();
        }

        public async Task<Pago> MarcarComoPagadoAsync(int idPago, DateOnly? fechaPago = null)
        {
            var pago = await _dbSet.FirstOrDefaultAsync(p => p.IdPago == idPago);
            if (pago == null)
                throw new NotFoundException($"No se encontró el pago con ID {idPago}");
            pago.Pagado = true;
            pago.FechaPago = fechaPago ?? DateOnly.FromDateTime(DateTime.Today);
            await _context.SaveChangesAsync();
            return pago;
        }
        public async Task<IEnumerable<Pago>> GetPagosPorVencerAsync(DateOnly fecha)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.FechaVencimiento != null && p.FechaVencimiento >= fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> GetPagosPorVencerEnDiasAsync(int dias)
        {
            var fechaLimite = DateOnly.FromDateTime(DateTime.Today.AddDays(dias));
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdEstudianteNavigation)
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdConvenioNavigation)
                .ThenInclude(c => c.IdEmpresaNavigation)
                .Where(p => p.FechaVencimiento != null && 
                           p.FechaVencimiento >= DateOnly.FromDateTime(DateTime.Today) && 
                           p.FechaVencimiento <= fechaLimite &&
                           (p.Pagado == null || p.Pagado == false))
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> BuscarAvanzadoAsync(PagosBusquedaAvanzadaDto filtro)
        {
            var pagos = await _dbSet
                .AsNoTracking()
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdEstudianteNavigation)
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdConvenioNavigation)
                .ThenInclude(c => c.IdEmpresaNavigation)
                .Where(p => p.IdPasantiaNavigation != null)
                .ToListAsync();

            bool IsStringValid(string? s) => !string.IsNullOrWhiteSpace(s) && s != "string";

            // Filtro por empresa (ID de la empresa asociada a la pasantía)
            if (filtro.IdEmpresa.HasValue)
            {
                pagos = pagos.Where(p => p.IdPasantiaNavigation?.IdConvenioNavigation?.IdEmpresaNavigation != null && 
                                        p.IdPasantiaNavigation.IdConvenioNavigation.IdEmpresaNavigation.IdEmpresa == filtro.IdEmpresa.Value).ToList();
            }

            // Filtro por estudiante (documento del estudiante asociado a la pasantía)
            if (IsStringValid(filtro.Estudiante))
            {
                pagos = pagos.Where(p => p.IdPasantiaNavigation?.IdEstudianteNavigation != null && 
                                        !string.IsNullOrEmpty(p.IdPasantiaNavigation.IdEstudianteNavigation.Documento) &&
                                        p.IdPasantiaNavigation.IdEstudianteNavigation.Documento.Contains(filtro.Estudiante, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtro por TramiteSudocu
            if (IsStringValid(filtro.TramiteSudocu))
            {
                pagos = pagos.Where(p => p.IdPasantiaNavigation != null && 
                                        !string.IsNullOrEmpty(p.IdPasantiaNavigation.TramiteSudocu) &&
                                        p.IdPasantiaNavigation.TramiteSudocu.Contains(filtro.TramiteSudocu!, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtro por estado del pago
            if (filtro.EstadoPago != null)
            {
                pagos = pagos.Where(p => p.Pagado == filtro.EstadoPago).ToList();
            }

            // Filtro por fecha de vencimiento
            if (!string.IsNullOrEmpty(filtro.FechaVencimiento))
            {
                if (DateOnly.TryParse(filtro.FechaVencimiento, out DateOnly fechaVencimiento))
                {
                    pagos = pagos.Where(p => p.FechaVencimiento == fechaVencimiento).ToList();
                }
            }

            return pagos;
        }

        public async Task<IEnumerable<object>> GetSugerenciasEmpresasAsync()
        {
            var empresas = await _dbSet
                .AsNoTracking()
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdConvenioNavigation)
                .ThenInclude(c => c.IdEmpresaNavigation)
                .Where(p => p.IdPasantiaNavigation != null && 
                           p.IdPasantiaNavigation.IdConvenioNavigation != null &&
                           p.IdPasantiaNavigation.IdConvenioNavigation.IdEmpresaNavigation != null)
                .Select(p => new
                {
                    value = p.IdPasantiaNavigation.IdConvenioNavigation.IdEmpresaNavigation.IdEmpresa,
                    label = p.IdPasantiaNavigation.IdConvenioNavigation.IdEmpresaNavigation.Nombre
                })
                .Distinct()
                .OrderBy(x => x.label)
                .ToListAsync();

            return empresas;
        }

        public async Task<IEnumerable<object>> GetSugerenciasEstudiantesAsync()
        {
            var estudiantes = await _dbSet
                .AsNoTracking()
                .Include(p => p.IdPasantiaNavigation)
                .ThenInclude(p => p.IdEstudianteNavigation)
                .Where(p => p.IdPasantiaNavigation != null && 
                           p.IdPasantiaNavigation.IdEstudianteNavigation != null)
                .Select(p => new
                {
                    value = p.IdPasantiaNavigation.IdEstudianteNavigation.Documento,
                    label = p.IdPasantiaNavigation.IdEstudianteNavigation.Documento
                })
                .Distinct()
                .OrderBy(x => x.label)
                .ToListAsync();

            return estudiantes;
        }

        public async Task<IEnumerable<string>> GetSugerenciasTramitesSudocuAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdPasantiaNavigation)
                .Where(p => p.IdPasantiaNavigation != null && 
                           !string.IsNullOrEmpty(p.IdPasantiaNavigation.TramiteSudocu))
                .Select(p => p.IdPasantiaNavigation.TramiteSudocu!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }
    }
}
