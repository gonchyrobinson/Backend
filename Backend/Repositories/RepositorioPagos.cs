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
    }
}
