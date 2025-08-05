using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IRepositorioEstudiantes : IRepository<Estudiante>
    {
        // Puedes agregar aquí métodos específicos para estudiantes si los necesitas
    }
}
