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
                .Where(p => p.IdConvenioNavigation == null || p.IdConvenioNavigation.FechaCaducidad == null || p.IdConvenioNavigation.FechaCaducidad > DateOnly.FromDateTime(DateTime.Now))
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
                })
                .OrderByDescending(c => c.Pasantia.IdPasantia);
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
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
        }
    }
}
