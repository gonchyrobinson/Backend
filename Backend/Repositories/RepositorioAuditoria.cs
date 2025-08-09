using Backend.Contexts;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repositories
{
    public class RepositorioAuditoria : Repository<Auditoria>, IRepositorioAuditoria
    {
        private readonly ApplicationDbContext _context;

        public RepositorioAuditoria(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auditoria>> BuscarAsync(Backend.DTOs.AuditoriaBuscarDto filtro)
        {
            // Validación de fechas
            if (filtro.FechaDesde != null && filtro.FechaHasta != null)
            {
                if (filtro.FechaDesde > filtro.FechaHasta)
                    throw new Backend.Exceptions.ValidationException("La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'", "Auditoria");
            }

            var query = _context.Auditoria.Include(a => a.IdUsuarioNavigation).AsQueryable();

            // Solo filtra si el valor no es nulo ni vacío
            if (!string.IsNullOrWhiteSpace(filtro.UsuarioNombre))
                query = query.Where(a => a.IdUsuarioNavigation != null && a.IdUsuarioNavigation.NombreUsuario == filtro.UsuarioNombre);

            if (!string.IsNullOrWhiteSpace(filtro.Accion))
                query = query.Where(a => a.TipoOperacion == filtro.Accion);

            if (filtro.FechaDesde != null)
                query = query.Where(a => a.FechaOperacion >= filtro.FechaDesde.Value);

            if (filtro.FechaHasta != null)
                query = query.Where(a => a.FechaOperacion <= filtro.FechaHasta.Value);

            var result = await query.ToListAsync();
            if (result == null || result.Count == 0)
                throw new Backend.Exceptions.NotFoundException("No se encontraron registros de auditoria con los filtros especificados.");
            return result;
        }
    }
}
