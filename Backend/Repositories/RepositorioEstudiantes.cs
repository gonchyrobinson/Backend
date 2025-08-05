using Backend.Interfaces;
using Backend.Contexts;
using Backend.Models;

namespace Backend.Repositories
{
    public class RepositorioEstudiantes : Repository<Estudiante>, IRepositorioEstudiantes
    {
        public RepositorioEstudiantes(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Estudiante>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro)
        {
            var estudiantes = await GetAllAsync();

            bool IsStringValid(string? s) => !string.IsNullOrWhiteSpace(s) && s != "string";

            if (IsStringValid(filtro.Nombre))
                estudiantes = estudiantes.Where(e => !string.IsNullOrEmpty(e.Nombre) && e.Nombre.ToLower().Contains(filtro.Nombre!.ToLower()));

            if (IsStringValid(filtro.Apellido))
                estudiantes = estudiantes.Where(e => !string.IsNullOrEmpty(e.Apellido) && e.Apellido.ToLower().Contains(filtro.Apellido!.ToLower()));

            if (IsStringValid(filtro.Documento))
                estudiantes = estudiantes.Where(e => !string.IsNullOrEmpty(e.Documento) && e.Documento.ToLower().Contains(filtro.Documento!.ToLower()));

            if (IsStringValid(filtro.Carrera))
                estudiantes = estudiantes.Where(e => !string.IsNullOrEmpty(e.Carrera) && e.Carrera.ToLower().Contains(filtro.Carrera!.ToLower()));

            if (IsStringValid(filtro.AreaTrabajo))
                estudiantes = estudiantes.Where(e => !string.IsNullOrEmpty(e.AreaTrabajo) && e.AreaTrabajo.ToLower().Contains(filtro.AreaTrabajo!.ToLower()));

            return estudiantes;
        }
    }
}