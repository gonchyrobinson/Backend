using Backend.Contexts;
using Backend.DTOs.ConvenioDtos;
using Backend.Exceptions;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RepositorioConvenios : Repository<Convenio>, IRepositorioConvenios
    {
        public RepositorioConvenios(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Convenio>> GetAllAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            return await _dbSet
                .AsNoTracking()
                .Where(c => !c.FechaCaducidad.HasValue || c.FechaCaducidad > hoy)
                .OrderBy(c => c.FechaCaducidad == null)
                .ThenBy(c => c.FechaCaducidad)
                .ToListAsync();
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
                        query = query.Where(c => c.IdEmpresaNavigation != null &&
                                                (c.IdEmpresaNavigation.Eliminado == null || c.IdEmpresaNavigation.Eliminado == false) &&
                                                c.IdEmpresaNavigation.Nombre != null && c.IdEmpresaNavigation.Nombre.Contains(filtro.NombreEmpresa));
                    if (!string.IsNullOrWhiteSpace(filtro.DocRepresentanteFacultad))
                        query = query.Where(c => c.DocRepresentanteFacultad != null && c.DocRepresentanteFacultad.Contains(filtro.DocRepresentanteFacultad));
                    if (!string.IsNullOrWhiteSpace(filtro.Carrera))
                    {
                        query = query.Where(c => c.Pasantia.Any(p =>
                            p.IdEstudianteNavigation != null &&
                            (p.IdEstudianteNavigation.Eliminado == null || p.IdEstudianteNavigation.Eliminado == false) &&
                            !string.IsNullOrEmpty(p.IdEstudianteNavigation.Carrera) &&
                            p.IdEstudianteNavigation.Carrera == filtro.Carrera
                        ));
                    }
                }

                var convenios = query
                    .AsNoTracking()
                    .Where(c => c.IdEmpresaNavigation == null ||
                              (c.IdEmpresaNavigation.Eliminado == null || c.IdEmpresaNavigation.Eliminado == false))
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
                throw new ValidationException("Error al listar convenios con empresa", ex, "Convenio");
            }
        }

        public async Task<bool> AsignarEmpresaAsync(AsignarEmpresaDto dto)
        {
            if (dto.ConvenioId <= 0 || dto.EmpresaId <= 0)
                throw new ValidationException("IDs de convenio y empresa deben ser mayores a cero", "Convenio");

            var convenio = await _dbSet.FindAsync(dto.ConvenioId);
            if (convenio == null)
                throw new NotFoundException($"Convenio con ID {dto.ConvenioId} no encontrado");

            convenio.IdEmpresa = dto.EmpresaId;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Error al asignar empresa al convenio", ex, "Convenio");
            }
        }

        public async Task<bool> CaducarConvenioAsync(int convenioId, DateOnly? fechaCaducidad)
        {
            if (convenioId <= 0)
                throw new ValidationException("El ID de convenio debe ser mayor a cero", "Convenio");

            var convenio = await _dbSet.FindAsync(convenioId);
            if (convenio == null)
                throw new NotFoundException($"Convenio con ID {convenioId} no encontrado");

            convenio.FechaCaducidad = fechaCaducidad ?? DateOnly.FromDateTime(DateTime.Now);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Error al caducar convenio", ex, "Convenio");
            }
        }

        public async Task<IEnumerable<object>> GetSugerenciasDropdownAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Select(c => new 
                {
                    value = c.IdConvenio,
                    label = $"EXP-FACET-{c.IdConvenio:D3}"
                })
                .OrderBy(x => x.value)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmpresaConvenioDropdownDto>> GetEmpresasConUltimoConvenioVigenteAsync()
        {
            var fechaActual = DateOnly.FromDateTime(DateTime.Now);
            
            // Obtener empresas con al menos un convenio vigente
            var empresasConConvenioVigente = await _dbSet
                .AsNoTracking()
                .Where(c => c.IdEmpresa.HasValue && 
                           c.IdEmpresaNavigation != null &&
                           (c.IdEmpresaNavigation.Eliminado == null || c.IdEmpresaNavigation.Eliminado == false) &&
                           (!c.FechaCaducidad.HasValue || c.FechaCaducidad > fechaActual))
                .GroupBy(c => new { 
                    c.IdEmpresa, 
                    NombreEmpresa = c.IdEmpresaNavigation!.Nombre 
                })
                .Select(g => new EmpresaConvenioDropdownDto
                {
                    IdEmpresa = g.Key.IdEmpresa!.Value,
                    NombreEmpresa = g.Key.NombreEmpresa ?? "Empresa sin nombre",
                    IdConvenio = g.OrderByDescending(c => c.FechaFirma ?? DateOnly.MinValue)
                                  .First().IdConvenio,
                    FechaInicio = g.OrderByDescending(c => c.FechaFirma ?? DateOnly.MinValue)
                                   .First().FechaFirma ?? DateOnly.MinValue
                })
                .OrderBy(x => x.NombreEmpresa)
                .ToListAsync();

            return empresasConConvenioVigente;
        }
    }
}
