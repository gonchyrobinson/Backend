using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Backend.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            // Detach any existing entity with the same key to avoid tracking conflicts
            var idProperty = GetIdProperty(entity);
            if (idProperty != null)
            {
                var id = idProperty.GetValue(entity);
                var existingEntity = await _dbSet.FindAsync(id);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).State = EntityState.Detached;
                }
            }
            
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private PropertyInfo? GetIdProperty(T entity)
        {
            var type = typeof(T);
            
            // Buscar propiedades que contengan "Id" en el nombre
            var idProperties = type.GetProperties()
                .Where(p => p.Name.ToLower().Contains("id") && p.PropertyType == typeof(int))
                .ToList();

            // Priorizar propiedades que empiecen con "Id"
            var primaryIdProperty = idProperties.FirstOrDefault(p => p.Name.StartsWith("Id"));
            if (primaryIdProperty != null)
                return primaryIdProperty;

            // Si no hay ninguna que empiece con "Id", devolver la primera que contenga "Id"
            return idProperties.FirstOrDefault();
        }
    }
}