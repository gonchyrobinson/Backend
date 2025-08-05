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
    }
}