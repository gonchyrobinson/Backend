using Backend.Interfaces;
using Backend.Models;
using Backend.Data;

namespace Backend.Repositories
{
    public class StudentRepository : Repository<Student>, IRepository<Student>
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}