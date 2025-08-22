using Backend.Contexts;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RepositorioEstudiantes : Repository<Estudiante>, IRepositorioEstudiantes
    {
        public RepositorioEstudiantes(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Estudiante>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro)
        {
            var estudiantes = await _dbSet
                .AsNoTracking()
                .Where(e => e.Eliminado == null || e.Eliminado == false)
                .ToListAsync();

            bool IsStringValid(string? s) => !string.IsNullOrWhiteSpace(s) && s != "string";

            if (IsStringValid(filtro.Documento))
                estudiantes = estudiantes.Where(e => !string.IsNullOrEmpty(e.Documento) && e.Documento.ToLower().Contains(filtro.Documento!.ToLower())).ToList();

            if (IsStringValid(filtro.Carrera))
                estudiantes = estudiantes.Where(e => !string.IsNullOrEmpty(e.Carrera) && e.Carrera.ToLower().Contains(filtro.Carrera!.ToLower())).ToList();

            // Agrupar por Documento y devolver solo un estudiante por documento
            estudiantes = estudiantes
                .GroupBy(e => e.Documento)
                .Select(g => g.First())
                .ToList();

            return estudiantes;
        }

        public async Task<IEnumerable<string>> GetSugerenciasNombresAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => (e.Eliminado == null || e.Eliminado == false) && !string.IsNullOrEmpty(e.Nombre))
                .Select(e => e.Nombre!)
                .Distinct()
                .OrderBy(nombre => nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetSugerenciasApellidosAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => (e.Eliminado == null || e.Eliminado == false) && !string.IsNullOrEmpty(e.Apellido))
                .Select(e => e.Apellido!)
                .Distinct()
                .OrderBy(apellido => apellido)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetDocumentosUnicos()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => (e.Eliminado == null || e.Eliminado == false) && 
                           !string.IsNullOrEmpty(e.Documento))
                .GroupBy(e => e.Documento)
                .Select(g => new 
                {
                    value = g.First().IdEstudiante,
                    label = g.Key
                })
                .OrderBy(x => x.label)
                .ToListAsync();
        }
    }
}