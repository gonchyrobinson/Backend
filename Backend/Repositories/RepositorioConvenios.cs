using Backend.DTOs;
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
        public async Task<IEnumerable<ConvenioEmpresaDto>> ListarConveniosConEmpresa(ConvenioEmpresaFiltroDto filtro)
        {
            try
            {
                var query = _dbSet.AsQueryable();

                if (filtro != null)
                {
                    if (filtro.FechaFirmaDesde.HasValue)
                        query = query.Where(c => c.FechaFirma >= filtro.FechaFirmaDesde);
                    if (filtro.FechaFirmaHasta.HasValue)
                        query = query.Where(c => c.FechaFirma <= filtro.FechaFirmaHasta);
                    if (filtro.FechaCaducidadDesde.HasValue)
                        query = query.Where(c => c.FechaCaducidad >= filtro.FechaCaducidadDesde);
                    if (filtro.FechaCaducidadHasta.HasValue)
                        query = query.Where(c => c.FechaCaducidad <= filtro.FechaCaducidadHasta);
                    if (!string.IsNullOrWhiteSpace(filtro.NombreEmpresa))
                        query = query.Where(c => c.IdEmpresaNavigation != null && c.IdEmpresaNavigation.Nombre.Contains(filtro.NombreEmpresa));
                    if (!string.IsNullOrWhiteSpace(filtro.DocRepresentanteFacultad))
                        query = query.Where(c => c.DocRepresentanteFacultad != null && c.DocRepresentanteFacultad.Contains(filtro.DocRepresentanteFacultad));
                }

                var convenios = query
                    .Select(c => new ConvenioEmpresaDto
                    {
                        IdConvenio = c.IdConvenio,
                        //Expediente = c.Expediente,
                        FechaFirma = c.FechaFirma,
                        FechaCaducidad = c.FechaCaducidad,
                        IdEmpresa = c.IdEmpresa,
                        NombreEmpresa = c.IdEmpresaNavigation != null ? c.IdEmpresaNavigation.Nombre : null,
                        RepresentanteEmpresa = c.RepresentanteEmpresa,
                        DomicilioLegal = c.DomicilioLegal,
                        DomicilioAlternativo = c.DomicilioAlternativo,
                        DocRepresentanteFacultad = c.DocRepresentanteFacultad,
                        Caracter = c.Caracter,
                        Sudocu = c.Sudocu
                    });
                return await convenios.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Backend.Exceptions.ValidationException("Error al listar convenios con empresa", ex, "Convenio");
            }
        }

        public async Task<bool> AsignarEmpresaAsync(AsignarEmpresaDto dto)
        {
            if (dto.ConvenioId <= 0 || dto.EmpresaId <= 0)
                throw new Backend.Exceptions.ValidationException("IDs de convenio y empresa deben ser mayores a cero", "Convenio");

            var convenio = await _dbSet.FindAsync(dto.ConvenioId);
            if (convenio == null)
                throw new Backend.Exceptions.NotFoundException($"Convenio con ID {dto.ConvenioId} no encontrado");

            convenio.IdEmpresa = dto.EmpresaId;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Backend.Exceptions.ValidationException("Error al asignar empresa al convenio", ex, "Convenio");
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
