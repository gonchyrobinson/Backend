using Backend.Contexts;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Repositories
{
    public class RepositorioPagos : Repository<Pago>, IRepositorioPagos
    {
        private readonly ApplicationDbContext _context;

        public RepositorioPagos(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pago> GetByPasantiaIdAsync(int idPasantia)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.IdPasantia == idPasantia);
            if (pago == null)
                throw new Backend.Exceptions.NotFoundException($"No se encontró un pago para la pasantía con ID {idPasantia}");
            return pago;
        }

        public async Task<Pago> MarcarComoPagadoAsync(int idPago, DateOnly? fechaPago = null)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.IdPago == idPago);
            if (pago == null)
                throw new Backend.Exceptions.NotFoundException($"No se encontró el pago con ID {idPago}");
            pago.Pagado = true;
            pago.FechaPago = fechaPago ?? DateOnly.FromDateTime(DateTime.Today);
            await _context.SaveChangesAsync();
            return pago;
        }
        public async Task<IEnumerable<Pago>> GetPagosPorVencerAsync(DateOnly fecha)
        {
            return await _context.Pagos
                .Where(p => p.FechaVencimiento != null && p.FechaVencimiento >= fecha)
                .ToListAsync();
        }
    }
}
