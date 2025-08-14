using Backend.DTOs;
using Microsoft.EntityFrameworkCore;
using Backend.Interfaces;
using Backend.Models;
using Backend.Contexts;
using Backend.Exceptions;

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
                .Include(p => p.IdEstudianteNavigation)
                .Include(p => p.IdConvenioNavigation)
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
                        dniTutorFacultad = p.dniTutorFacultad,
                        FechaInicio = p.FechaInicio,
                        FechaFin = p.FechaFin,
                        TipoAcuerdo = p.TipoAcuerdo,
                        Observaciones = p.Observaciones,
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
            return await _dbSet.Where(p => p.IdConvenio == convenioId).ToListAsync();
        }

        public async Task<IEnumerable<Pasantia>> GetByEstudianteIdAsync(int estudianteId)
        {
            return await _dbSet.Where(p => p.IdEstudiante == estudianteId).ToListAsync();
        }
        public async Task AgregarPagoAsync(Pago pago)
        {
            var context = (ApplicationDbContext)_context;
            context.Pagos.Add(pago);
            await context.SaveChangesAsync();
        }
    }
}
