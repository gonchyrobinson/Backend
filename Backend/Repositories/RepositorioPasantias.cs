using Backend.Contexts;
using Backend.DTOs.PasantiaDtos;
using Backend.DTOs.StudentDtos;
using Backend.DTOs.ConvenioDtos;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RepositorioPasantias : Repository<Pasantia>, IRepositorioPasantias
    {
        public RepositorioPasantias(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<bool> DeleteAsync(int id)

        {

            // Buscar la pasantía

            var pasantia = await _dbSet.FindAsync(id);

            if (pasantia == null)

                return false;

            // Eliminar pagos asociados

            var pagos = _context.Pagos.Where(p => p.IdPasantia == id);

            _context.Pagos.RemoveRange(pagos);

            await _context.SaveChangesAsync();

            // Llamar al método base para eliminar la pasantía (lógico o físico)

            return await base.DeleteAsync(id);

        }

        // Métodos específicos para pasantías pueden agregarse aquí
        public async Task<IEnumerable<PasantiaDetalleDto>> GetAllDetalleAsync()
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Include(p => p.IdConvenioNavigation)
                .Where(p => p.IdEstudianteNavigation == null ||
                          (p.IdEstudianteNavigation.Eliminado == null || p.IdEstudianteNavigation.Eliminado == false) && (p.IdConvenioNavigation!.FechaCaducidad == null || p.IdConvenioNavigation.FechaCaducidad > DateOnly.FromDateTime(DateTime.Now)))
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
                        DniTutorEmpresa = p.DniTutorEmpresa,
                        TutorFacultad = p.TutorFacultad,
                        DniTutorFacultad = p.DniTutorFacultad,
                        FechaInicio = p.FechaInicio,
                        FechaFin = p.FechaFin,
                        TipoAcuerdo = p.TipoAcuerdo,
                        Observaciones = p.Observaciones,
                        TramiteSudocu = p.TramiteSudocu,
                        FrecuenciaPago = p.FrecuenciaPago,
                        AreaTrabajo = p.AreaTrabajo
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

        public async Task<IEnumerable<PasantiaShowTableDto>> GetAllPasantiasShowTableAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            var query = _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Include(p => p.IdConvenioNavigation)
                .ThenInclude(c => c!.IdEmpresaNavigation)
                .Where(p => p.IdEstudianteNavigation == null ||
                          (p.IdEstudianteNavigation.Eliminado == null || p.IdEstudianteNavigation.Eliminado == false))
                .Select(p => new
                {
                    IdPasantia = p.IdPasantia,
                    TramiteSudocu = p.TramiteSudocu,
                    Estudiante = p.IdEstudianteNavigation != null
                        ? $"{p.IdEstudianteNavigation.Apellido}, {p.IdEstudianteNavigation.Nombre}"
                        : "Sin estudiante",
                    Empresa = p.IdConvenioNavigation != null && p.IdConvenioNavigation.IdEmpresaNavigation != null
                        ? p.IdConvenioNavigation.IdEmpresaNavigation.Nombre ?? "Sin nombre"
                        : "Sin empresa",
                    TipoAcuerdo = p.TipoAcuerdo ?? "No especificado",
                    Estado = (!p.FechaFin.HasValue || p.FechaFin > hoy) ? "Activa" : "Finalizada",
                    FechaInicio = p.FechaInicio,
                    FechaFin = p.FechaFin,
                    // Campos auxiliares para ordenamiento
                    EsActiva = (!p.FechaFin.HasValue || p.FechaFin > hoy),
                    EstudianteOrden = p.IdEstudianteNavigation != null
                        ? (p.IdEstudianteNavigation.Apellido ?? "").Trim() + ", " + (p.IdEstudianteNavigation.Nombre ?? "").Trim()
                        : "Sin estudiante"
                })
                .OrderBy(p => p.EsActiva ? 0 : 1) // Primero las activas
                .ThenBy(p => p.FechaInicio.HasValue ? 0 : 1) // Primero las que tienen fecha de inicio
                .ThenByDescending(p => p.FechaInicio) // Fecha de inicio descendente (más reciente primero)
                .ThenBy(p => p.EstudianteOrden); // Finalmente por estudiante alfabéticamente
            
            var result = await query.ToListAsync();
            
            return result.Select(p => new PasantiaShowTableDto
            {
                IdPasantia = p.IdPasantia,
                TramiteSudocu = p.TramiteSudocu,
                Estudiante = p.Estudiante,
                Empresa = p.Empresa,
                TipoAcuerdo = p.TipoAcuerdo,
                Estado = p.Estado,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin
            });
        }

        public async Task<IEnumerable<Pasantia>> GetByConvenioIdAsync(int convenioId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Where(p => p.IdConvenio == convenioId &&
                          (p.IdEstudianteNavigation == null ||
                           p.IdEstudianteNavigation.Eliminado == null ||
                           p.IdEstudianteNavigation.Eliminado == false))
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
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetSugerenciasTramitesAsync()
        {
            // Obtener todos los TramiteSudocu que no sean nulos
            return await _dbSet
                .AsNoTracking()
                .Where(p => !string.IsNullOrEmpty(p.TramiteSudocu))
                .Select(p => p.TramiteSudocu!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetSugerenciasNumerosTramiteAsync()
        {
            // Obtener todos los TramiteSudocu que no sean nulos
            return await _dbSet
                .AsNoTracking()
                .Where(p => !string.IsNullOrEmpty(p.TramiteSudocu))
                .Select(p => p.TramiteSudocu!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetSugerenciasDropdownAsync()
        {
            // Obtener todas las pasantías con información básica para dropdown
            var pasantias = await _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Include(p => p.IdConvenioNavigation)
                .ThenInclude(c => c.IdEmpresaNavigation)
                .Where(p => p.IdEstudianteNavigation == null ||
                          (p.IdEstudianteNavigation.Eliminado == null || p.IdEstudianteNavigation.Eliminado == false))
                .Select(p => new
                {
                    value = p.IdPasantia,
                    tramiteSudocu = p.TramiteSudocu,
                    estudianteDocumento = p.IdEstudianteNavigation != null ? p.IdEstudianteNavigation.Documento : "Sin estudiante",
                    empresaNombre = p.IdConvenioNavigation != null && p.IdConvenioNavigation.IdEmpresaNavigation != null 
                        ? p.IdConvenioNavigation.IdEmpresaNavigation.Nombre 
                        : "Sin empresa"
                })
                .OrderBy(x => x.value)
                .ToListAsync();

            // Formatear las etiquetas después de obtener los datos
            return pasantias.Select(p => new
            {
                value = p.value,
                label = $"{(!string.IsNullOrEmpty(p.tramiteSudocu) ? p.tramiteSudocu : $"ID-{p.value}")} - {p.estudianteDocumento} - {p.empresaNombre}"
            });
        }

        public async Task<IEnumerable<PasantiaShowTableDto>> BuscarAvanzadoAsync(PasantiaBusquedaAvanzadaDto filtro)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            
            var pasantias = await _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Include(p => p.IdConvenioNavigation)
                .ThenInclude(c => c!.IdEmpresaNavigation)
                .Where(p => p.IdEstudianteNavigation == null ||
                          (p.IdEstudianteNavigation.Eliminado == null || p.IdEstudianteNavigation.Eliminado == false))
                .ToListAsync();

            bool IsStringValid(string? s) => !string.IsNullOrWhiteSpace(s) && s != "string";

            // Filtro por TramiteSudocu - búsqueda directa en el campo
            if (IsStringValid(filtro.TramiteSudocu))
            {
                var tramite = filtro.TramiteSudocu!.Trim();
                // Búsqueda directa en el campo TramiteSudocu
                pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.TramiteSudocu) && 
                                               p.TramiteSudocu.Contains(tramite, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtro por tipo de acuerdo
            if (IsStringValid(filtro.Tipo))
            {
                if (filtro.Tipo!.Equals("otro", StringComparison.OrdinalIgnoreCase))
                {
                    // Para "otro", incluir todos los tipos que NO sean PPS ni Pasantia
                    pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.TipoAcuerdo) && 
                                                   !p.TipoAcuerdo.Equals("PPS", StringComparison.OrdinalIgnoreCase) && 
                                                   !p.TipoAcuerdo.Equals("Pasantia", StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    // Para tipos específicos, búsqueda exacta
                    pasantias = pasantias.Where(p => !string.IsNullOrEmpty(p.TipoAcuerdo) && 
                                                   p.TipoAcuerdo.Equals(filtro.Tipo, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }

            // Filtro por estudiante (documento)
            if (IsStringValid(filtro.Estudiante))
                pasantias = pasantias.Where(p => p.IdEstudianteNavigation != null && 
                                               !string.IsNullOrEmpty(p.IdEstudianteNavigation.Documento) &&
                                               p.IdEstudianteNavigation.Documento.Contains(filtro.Estudiante!, StringComparison.OrdinalIgnoreCase)).ToList();

            // Filtro por empresa (nombre)
            if (IsStringValid(filtro.Empresa))
                pasantias = pasantias.Where(p => p.IdConvenioNavigation != null && 
                                               p.IdConvenioNavigation.IdEmpresaNavigation != null &&
                                               !string.IsNullOrEmpty(p.IdConvenioNavigation.IdEmpresaNavigation.Nombre) &&
                                               p.IdConvenioNavigation.IdEmpresaNavigation.Nombre.Contains(filtro.Empresa!, StringComparison.OrdinalIgnoreCase)).ToList();

            // Filtro por vigencia
            if (filtro.Vigente != null)
            {
                if (filtro.Vigente.Value)
                {
                    // Pasantías vigentes: FechaFin > hoy o nula
                    pasantias = pasantias.Where(p => !p.FechaFin.HasValue || p.FechaFin > hoy).ToList();
                }
                else
                {
                    // Pasantías no vigentes: FechaFin < hoy
                    pasantias = pasantias.Where(p => p.FechaFin.HasValue && p.FechaFin <= hoy).ToList();
                }
            }

            // Filtro por carrera
            if (IsStringValid(filtro.Carrera))
                pasantias = pasantias.Where(p => p.IdEstudianteNavigation != null && 
                                               !string.IsNullOrEmpty(p.IdEstudianteNavigation.Carrera) &&
                                               p.IdEstudianteNavigation.Carrera.Equals(filtro.Carrera, StringComparison.OrdinalIgnoreCase)).ToList();

            // Convertir a DTOs con ordenamiento
            return pasantias.Select(p => new
            {
                IdPasantia = p.IdPasantia,
                TramiteSudocu = p.TramiteSudocu,
                Estudiante = p.IdEstudianteNavigation != null
                    ? $"{p.IdEstudianteNavigation.Apellido}, {p.IdEstudianteNavigation.Nombre}"
                    : "Sin estudiante",
                Empresa = p.IdConvenioNavigation != null && p.IdConvenioNavigation.IdEmpresaNavigation != null
                    ? p.IdConvenioNavigation.IdEmpresaNavigation.Nombre ?? "Sin nombre"
                    : "Sin empresa",
                TipoAcuerdo = p.TipoAcuerdo ?? "No especificado",
                Estado = (!p.FechaFin.HasValue || p.FechaFin > hoy) ? "Activa" : "Finalizada",
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                // Campos auxiliares para ordenamiento
                EsActiva = (!p.FechaFin.HasValue || p.FechaFin > hoy),
                EstudianteOrden = p.IdEstudianteNavigation != null
                    ? (p.IdEstudianteNavigation.Apellido ?? "").Trim() + ", " + (p.IdEstudianteNavigation.Nombre ?? "").Trim()
                    : "Sin estudiante"
            })
            .OrderBy(p => p.EsActiva ? 0 : 1) // Primero las activas
            .ThenBy(p => p.FechaInicio.HasValue ? 0 : 1) // Primero las que tienen fecha de inicio
            .ThenByDescending(p => p.FechaInicio) // Fecha de inicio descendente (más reciente primero)
            .ThenBy(p => p.EstudianteOrden) // Finalmente por estudiante alfabéticamente
            .Select(p => new PasantiaShowTableDto
            {
                IdPasantia = p.IdPasantia,
                TramiteSudocu = p.TramiteSudocu,
                Estudiante = p.Estudiante,
                Empresa = p.Empresa,
                TipoAcuerdo = p.TipoAcuerdo,
                Estado = p.Estado,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin
            });
        }

        public async Task<IEnumerable<Pasantia>> GetAllWithStudentNavigationAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Where(p => p.IdEstudianteNavigation == null ||
                          (p.IdEstudianteNavigation.Eliminado == null || p.IdEstudianteNavigation.Eliminado == false))
                .ToListAsync();
        }

        public async Task<Pasantia?> GetByIdWithStudentNavigationAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.IdEstudianteNavigation)
                .Where(p => p.IdPasantia == id &&
                          (p.IdEstudianteNavigation == null ||
                           p.IdEstudianteNavigation.Eliminado == null ||
                           p.IdEstudianteNavigation.Eliminado == false))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Pasantia>> GetPasantiasPorVencerAsync(DateOnly fecha)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.FechaFin != null && p.FechaFin >= fecha)
                .ToListAsync();
        }

        // Pasantías por vencer en X días desde hoy
        public async Task<IEnumerable<Pasantia>> GetPasantiasPorVencerEnDiasAsync(int dias)
        {
            var fechaLimite = DateOnly.FromDateTime(DateTime.Today.AddDays(dias));
            var fechaActual = DateOnly.FromDateTime(DateTime.Today);
            
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.FechaFin != null && 
                           p.FechaFin >= fechaActual && 
                           p.FechaFin <= fechaLimite)
                .OrderBy(p => p.FechaFin)
                .ToListAsync();
        }
    }
}