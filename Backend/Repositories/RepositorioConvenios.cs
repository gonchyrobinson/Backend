using Backend.Interfaces;
using Backend.Models;
using Backend.Contexts;

namespace Backend.Repositories
{
    public class RepositorioConvenios : Repository<Convenio>, IRepositorioConvenios
    {
        public RepositorioConvenios(ApplicationDbContext context) : base(context)
        {
        }
        // Métodos específicos para Convenio pueden agregarse aquí
    }
}
