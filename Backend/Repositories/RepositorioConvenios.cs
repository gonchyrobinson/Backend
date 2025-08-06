using Backend.Interfaces;
using Backend.Models;
using Backend.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RepositorioConvenios : Repository<Convenio>, IRepositorioConvenios
    {
        public RepositorioConvenios(ApplicationDbContext context) : base(context)
        {
        }

        // Listar convenios junto a empresa (nombre)
        public async Task<IEnumerable<Backend.DTOs.ConvenioEmpresaDto>> ListarConveniosConEmpresaAsync()
        {
            try
            {
                var convenios = _dbSet
                    .Select(c => new Backend.DTOs.ConvenioEmpresaDto
                    {
                        IdConvenio = c.IdConvenio,
                        Expediente = c.Expediente,
                        FechaFirma = c.FechaFirma,
                        FechaCaducidad = c.FechaCaducidad,
                        IdEmpresa = c.IdEmpresa,
                        NombreEmpresa = c.IdEmpresaNavigation != null ? c.IdEmpresaNavigation.Nombre : null,
                        RepresentanteEmpresa = c.RepresentanteEmpresa
                    });
                return await convenios.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Backend.Exceptions.ValidationException("Error al listar convenios con empresa", ex, "Convenio");
            }
        }

        public async Task<bool> CaducarConvenioAsync(int convenioId, DateOnly? fechaCaducidad)
        {
            if (convenioId <= 0)
                throw new Backend.Exceptions.ValidationException("El ID de convenio debe ser mayor a cero", "Convenio");

            var convenio = await _dbSet.FindAsync(convenioId);
            if (convenio == null)
                throw new Backend.Exceptions.NotFoundException($"Convenio con ID {convenioId} no encontrado");

            convenio.FechaCaducidad = fechaCaducidad ?? DateOnly.FromDateTime(DateTime.Now);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Backend.Exceptions.ValidationException("Error al caducar convenio", ex, "Convenio");
            }
        }
    }
}
