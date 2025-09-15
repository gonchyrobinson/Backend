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
                .Include(c => c.IdEmpresaNavigation)
                .Where(c => 
                    (!c.FechaCaducidad.HasValue || c.FechaCaducidad > hoy) &&
                    (c.IdEmpresaNavigation == null || 
                     (c.IdEmpresaNavigation.Eliminado == null || c.IdEmpresaNavigation.Eliminado == false)))
                .OrderBy(c => c.FechaCaducidad == null)
                .ThenBy(c => c.FechaCaducidad)
                .ToListAsync();
        }

        public override async Task<Convenio?> GetByIdAsync(int id)
        {
            var convenio = await _dbSet
                .Include(c => c.IdEmpresaNavigation)
                .FirstOrDefaultAsync(c => c.IdConvenio == id);

            if (convenio == null)
                throw new NotFoundException($"Convenio con ID {id} no encontrado.");

            // Si la empresa está lógicamente eliminada, ocultamos su información
            // pero mantenemos la entidad trackeada para compatibilidad con base repository
            if (convenio.IdEmpresaNavigation != null && 
                convenio.IdEmpresaNavigation.Eliminado == true)
            {
                // Detach la navegación eliminada para evitar conflictos
                _context.Entry(convenio.IdEmpresaNavigation).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                convenio.IdEmpresaNavigation = null;
            }

            return convenio;
        }

        // Listar convenios junto a empresa (nombre)
        public async Task<IEnumerable<ConvenioEmpresaDto>> ListarConveniosConEmpresa(ConvenioEmpresaFiltroDto filtro)
        {
            try
            {
                var query = _dbSet.AsQueryable();

                if (filtro != null)
                {
                    if (!string.IsNullOrWhiteSpace(filtro.NombreEmpresa))
                        query = query.Where(c => c.IdEmpresaNavigation != null &&
                                                (c.IdEmpresaNavigation.Eliminado == null || c.IdEmpresaNavigation.Eliminado == false) &&
                                                c.IdEmpresaNavigation.Nombre != null && c.IdEmpresaNavigation.Nombre.Contains(filtro.NombreEmpresa));
                    
                    if (!string.IsNullOrWhiteSpace(filtro.ExpedienteSudocu))
                        query = query.Where(c => c.ExpedienteSudocu != null && c.ExpedienteSudocu.Contains(filtro.ExpedienteSudocu));
                    
                    // Vigencia: true = convenios vigentes (FechaCaducidad > hoy o nula), false = no vigentes (FechaCaducidad < hoy)
                    if (filtro.Vigencia != null)
                    {
                        var hoy = DateOnly.FromDateTime(DateTime.Now);
                        if (filtro.Vigencia.Value)
                        {
                            // Convenios vigentes: FechaCaducidad > hoy o nula
                            query = query.Where(c => !c.FechaCaducidad.HasValue || c.FechaCaducidad > hoy);
                        }
                        else
                        {
                            // Convenios no vigentes: FechaCaducidad < hoy
                            query = query.Where(c => c.FechaCaducidad.HasValue && c.FechaCaducidad <= hoy);
                        }
                    }
                }

                var convenios = query
                    .AsNoTracking()
                    .Where(c => c.IdEmpresaNavigation == null ||
                              (c.IdEmpresaNavigation.Eliminado == null || c.IdEmpresaNavigation.Eliminado == false))
                    .Select(c => new ConvenioEmpresaDto
                    {
                        IdConvenio = c.IdConvenio,
                        FechaInicio = c.FechaInicio,
                        FechaCaducidad = c.FechaCaducidad,
                        IdEmpresa = c.IdEmpresa,
                        NombreEmpresa = c.IdEmpresaNavigation != null ? c.IdEmpresaNavigation.Nombre : null,
                        RepresentanteEmpresa = c.RepresentanteEmpresa,
                        NroAcuerdoMarco = c.NroAcuerdoMarco,
                        DomicilioLegal = c.DomicilioLegal,
                        DocumentoDecano = c.DocumentoDecano,
                        TipoAcuerdo = c.TipoAcuerdo,
                        ExpedienteSudocu = c.ExpedienteSudocu
                    })
                    .OrderBy(c => 
                        // Primero los vigentes (FechaCaducidad nula o > hoy)
                        (!c.FechaCaducidad.HasValue || c.FechaCaducidad > DateOnly.FromDateTime(DateTime.Now)) ? 0 : 1)
                    .ThenBy(c => (c.NombreEmpresa ?? "").Trim()); // Luego por nombre de empresa alfabéticamente sin espacios
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
                    IdConvenio = g.OrderByDescending(c => c.FechaInicio ?? DateOnly.MinValue)
                                  .First().IdConvenio,
                    FechaInicio = g.OrderByDescending(c => c.FechaInicio ?? DateOnly.MinValue)
                                   .First().FechaInicio ?? DateOnly.MinValue
                })
                .OrderBy(x => x.NombreEmpresa)
                .ToListAsync();

            return empresasConConvenioVigente;
        }

        // Convenios por vencer en X días desde hoy
        public async Task<IEnumerable<ConvenioEmpresaDto>> GetConveniosPorVencerEnDiasAsync(int dias)
        {
            var fechaLimite = DateOnly.FromDateTime(DateTime.Today.AddDays(dias));
            var fechaActual = DateOnly.FromDateTime(DateTime.Today);
            
            var convenios = await _dbSet
                .AsNoTracking()
                .Include(c => c.IdEmpresaNavigation)
                .Where(c => c.FechaCaducidad != null && 
                           c.FechaCaducidad >= fechaActual && 
                           c.FechaCaducidad <= fechaLimite)
                .OrderBy(c => c.FechaCaducidad)
                .Select(c => new ConvenioEmpresaDto
                {
                    IdConvenio = c.IdConvenio,
                    FechaInicio = c.FechaInicio,
                    FechaCaducidad = c.FechaCaducidad,
                    IdEmpresa = c.IdEmpresa,
                    NombreEmpresa = c.IdEmpresaNavigation != null ? c.IdEmpresaNavigation.Nombre : null,
                    RepresentanteEmpresa = c.RepresentanteEmpresa,
                    NroAcuerdoMarco = c.NroAcuerdoMarco,
                    DomicilioLegal = c.DomicilioLegal,
                    DocumentoDecano = c.DocumentoDecano,
                    TipoAcuerdo = c.TipoAcuerdo,
                    ExpedienteSudocu = c.ExpedienteSudocu
                })
                .ToListAsync();

            return convenios;
        }
    }
}
