using Backend.Contexts;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RepositorioPasantias : Repository<Pasantia>, IRepositorioPasantias
    {
        public RepositorioPasantias(ApplicationDbContext context) : base(context)
        {
        }

        // Métodos específicos para pasantías pueden agregarse aquí
        public async Task<IEnumerable<PasantiaDetalleDto>> GetAllDetalleAsync()
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Include(p => p.IdConvenioNavigation)
                .Where(p => p.IdEstudianteNavigation == null || 
                          (p.IdEstudianteNavigation.Eliminado == null || p.IdEstudianteNavigation.Eliminado == false))
                .Select(p => new PasantiaDetalleDto
                {
                    Pasantia = new PasantiaDto
                    {
                        IdPasantia = p.IdPasantia,
                        IdEstudiante = p.IdEstudiante,
                        IdConvenio = p.IdConvenio,
                        AsignacionMensual = p.AsignacionMensual,
                        ObraSocial = p.ObraSocial,
                        Art = p.Art,
                        TutorEmpresa = p.TutorEmpresa,
                        TutorFacultad = p.TutorFacultad,
                        DniTutorFacultad = p.DniTutorFacultad,
                        FechaInicio = p.FechaInicio,
                        FechaFin = p.FechaFin,
                        TipoAcuerdo = p.TipoAcuerdo,
                        Observaciones = p.Observaciones,
                        Sudocu = p.Sudocu,
                        FrecuenciaPago = p.FrecuenciaPago,
                    },
                    Estudiante = p.IdEstudianteNavigation != null ? new StudentDto
                    {
                        IdEstudiante = p.IdEstudianteNavigation.IdEstudiante,
                        Nombre = p.IdEstudianteNavigation.Nombre,
                        Apellido = p.IdEstudianteNavigation.Apellido,
                        Email = p.IdEstudianteNavigation.Email,
                        Carrera = p.IdEstudianteNavigation.Carrera
                    } : null,
                    Convenio = p.IdConvenioNavigation != null ? new ConvenioDto
                    {
                        IdConvenio = p.IdConvenioNavigation.IdConvenio,
                        IdEmpresa = p.IdConvenioNavigation.IdEmpresa
                    } : null
                });
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Pasantia>> GetByConvenioIdAsync(int convenioId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.IdConvenio == convenioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pasantia>> GetByEstudianteIdAsync(int estudianteId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Where(p => p.IdEstudiante == estudianteId && 
                          (p.IdEstudianteNavigation == null || 
                           p.IdEstudianteNavigation.Eliminado == null || 
                           p.IdEstudianteNavigation.Eliminado == false))
                .ToListAsync();
        }
        public async Task AgregarPagoAsync(Pago pago)
        {
            var context = (ApplicationDbContext)_context;
            context.Pagos.Add(pago);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetSugerenciasTramitesAsync()
        {
            // Obtener todos los IDs de pasantías y generar los trámites
            var pasantiaIds = await _dbSet
                .AsNoTracking()
                .Select(p => p.IdPasantia)
                .OrderBy(id => id)
                .ToListAsync();

            // Generar los números de trámite usando la misma lógica del modelo
            return pasantiaIds.Select(id => $"TRA-FACET-{id:D3}").ToList();
        }

        public async Task<IEnumerable<Pasantia>> BuscarAvanzadoAsync(PasantiaBusquedaAvanzadaDto filtro)
        {
            var pasantias = await _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Include(p => p.IdConvenioNavigation)
                .Where(p => p.IdEstudianteNavigation == null || 
                          (p.IdEstudianteNavigation.Eliminado == null || p.IdEstudianteNavigation.Eliminado == false))
                .ToListAsync();

            bool IsStringValid(string? s) => !string.IsNullOrWhiteSpace(s) && s != "string";

            // Filtro por trámite - extraer ID del formato TRA-FACET-XXX
            if (IsStringValid(filtro.Tramite))
            {
                var tramite = filtro.Tramite!.Trim();
                // Si el trámite tiene el formato TRA-FACET-XXX, extraer el ID
                if (tramite.StartsWith("TRA-FACET-", StringComparison.OrdinalIgnoreCase))
                {
                    var idPart = tramite.Substring("TRA-FACET-".Length);
                    if (int.TryParse(idPart, out int tramiteId))
                    {
                        pasantias = pasantias.Where(p => p.IdPasantia == tramiteId).ToList();
                    }
                }
                else
                {
                    // Búsqueda parcial en el formato generado
                    pasantias = pasantias.Where(p => $"TRA-FACET-{p.IdPasantia:D3}".Contains(tramite, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }

            if (IsStringValid(filtro.ObraSocial))
                pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.ObraSocial) && p.ObraSocial.ToLower().Contains(filtro.ObraSocial!.ToLower())).ToList();

            if (IsStringValid(filtro.Art))
                pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.Art) && p.Art.ToLower().Contains(filtro.Art!.ToLower())).ToList();

            if (IsStringValid(filtro.TutorEmpresa))
                pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.TutorEmpresa) && p.TutorEmpresa.ToLower().Contains(filtro.TutorEmpresa!.ToLower())).ToList();

            if (IsStringValid(filtro.TutorFacultad))
                pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.TutorFacultad) && p.TutorFacultad.ToLower().Contains(filtro.TutorFacultad!.ToLower())).ToList();

            if (IsStringValid(filtro.TipoAcuerdo))
                pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.TipoAcuerdo) && p.TipoAcuerdo.Equals(filtro.TipoAcuerdo, StringComparison.OrdinalIgnoreCase)).ToList();

            // Nota: Estado no existe en el modelo Pasantia actualmente
            // if (IsStringValid(filtro.Estado))
            //     pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.Estado) && p.Estado.Equals(filtro.Estado, StringComparison.OrdinalIgnoreCase)).ToList();

            // Filtros de fecha de inicio
            if (filtro.FechaInicioDesde.HasValue)
                pasantias = pasantias.Where(p => p.FechaInicio.HasValue && p.FechaInicio.Value >= filtro.FechaInicioDesde.Value).ToList();

            if (filtro.FechaInicioHasta.HasValue)
                pasantias = pasantias.Where(p => p.FechaInicio.HasValue && p.FechaInicio.Value <= filtro.FechaInicioHasta.Value).ToList();

            // Filtros de fecha de fin
            if (filtro.FechaFinDesde.HasValue)
                pasantias = pasantias.Where(p => p.FechaFin.HasValue && p.FechaFin.Value >= filtro.FechaFinDesde.Value).ToList();

            if (filtro.FechaFinHasta.HasValue)
                pasantias = pasantias.Where(p => p.FechaFin.HasValue && p.FechaFin.Value <= filtro.FechaFinHasta.Value).ToList();

            // Filtros por ID
            if (filtro.IdEstudiante.HasValue)
                pasantias = pasantias.Where(p => p.IdEstudiante == filtro.IdEstudiante.Value).ToList();

            if (filtro.IdConvenio.HasValue)
                pasantias = pasantias.Where(p => p.IdConvenio == filtro.IdConvenio.Value).ToList();

            return pasantias;
        }
    }
}
