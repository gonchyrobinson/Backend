using Backend.Contexts;
using Backend.Models;
using Backend.Interfaces;

namespace Backend.Repositories
{
    public class RepositorioEmpresas : Repository<Empresa>, IRepository<Empresa>
    {
        public RepositorioEmpresas(ApplicationDbContext context) : base(context)
        {
        }
    }
}
