using Backend.Interfaces;
using Backend.Contexts;
using Backend.Models;

namespace Backend.Repositories
{
    public class StudentRepository : Repository<Student>, IRepository<Student>
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}