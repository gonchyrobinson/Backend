using Backend.Contexts;
using Backend.DTOs.EmpresaDtos;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RepositorioEmpresas : Repository<Empresa>, IRepositorioEmpresas
    {
        public RepositorioEmpresas(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Empresa>> GetAllAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            
            // Primero obtenemos todas las empresas no eliminadas
            var empresas = await _dbSet
                .AsNoTracking()
                .Where(e => e.Eliminado == null || e.Eliminado == false)
                .Where(e => !string.IsNullOrEmpty(e.Nombre)) // Filtrar nombres no nulos/vacíos
                .ToListAsync();

            // Luego filtramos en memoria las que tienen al menos una letra
            return empresas
                .Where(e => e.Nombre!.Any(char.IsLetter))
                .OrderBy(e => 
                    // Primero las vigentes (FechaFin nula o > hoy)
                    (!e.FechaFin.HasValue || e.FechaFin > today) ? 0 : 1)
                .ThenBy(e => e.Nombre!.Any(char.IsLetter) ? 0 : 1) // Primero las que tienen letras en el nombre
                .ThenBy(e => e.Nombre!.Trim()) // Ordenar por nombre alfabético sin espacios al principio
                .ToList();
        }

        public async Task<IEnumerable<Empresa>> BuscarAvanzadoAsync(EmpresaBusquedaAvanzadaDto filtro)
        {
            var empresas = await _dbSet
                .AsNoTracking()
                .Where(e => e.Eliminado == null || e.Eliminado == false)
                .ToListAsync();

            bool IsStringValid(string? s) => !string.IsNullOrWhiteSpace(s) && s != "string";

            if (IsStringValid(filtro.Nombre))
                empresas = empresas.Where(e => !string.IsNullOrEmpty(e.Nombre) && e.Nombre.ToLower().Contains(filtro.Nombre!.ToLower())).ToList();

            // Only Nombre filtering remains after DTO reduction

            return empresas;
        }

        public async Task<IEnumerable<string>> GetSugerenciasNombresAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => (e.Eliminado == null || e.Eliminado == false) && !string.IsNullOrEmpty(e.Nombre))
                .Select(e => e.Nombre!)
                .Distinct()
                .OrderBy(nombre => nombre.Trim()) // Ordenar sin espacios al principio y final
                .ToListAsync();
        }
    }
}
