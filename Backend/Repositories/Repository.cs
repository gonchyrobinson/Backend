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
            try
            {
                // Si la entidad tiene la propiedad Eliminado, filtrar los eliminados lógicos
                var eliminadoProp = typeof(T).GetProperty("Eliminado");
                if (eliminadoProp != null && eliminadoProp.PropertyType == typeof(bool?))
                {
                    // Construir expresión dinámica para filtrar Eliminado == null || Eliminado == false
                    var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "e");
                    var prop = System.Linq.Expressions.Expression.Property(param, eliminadoProp);
                    var nullConst = System.Linq.Expressions.Expression.Constant(null, typeof(bool?));
                    var falseConst = System.Linq.Expressions.Expression.Constant(false, typeof(bool?));
                    var isNull = System.Linq.Expressions.Expression.Equal(prop, nullConst);
                    var isFalse = System.Linq.Expressions.Expression.Equal(prop, falseConst);
                    var or = System.Linq.Expressions.Expression.OrElse(isNull, isFalse);
                    var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(or, param);
                    return await _dbSet.Where(lambda).ToListAsync();
                }
                return await _dbSet.ToListAsync();
            }
            catch (ArgumentException ex)
            {
                throw new Exceptions.AppException($"Error de argumentos: {ex.Message}", ex);
            }
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new Backend.Exceptions.NotFoundException($"Entidad de tipo {typeof(T).Name} con ID {id} no encontrada.");

            var eliminadoProp = typeof(T).GetProperty("Eliminado");
            if (eliminadoProp != null && eliminadoProp.PropertyType == typeof(bool?))
            {
                var eliminadoValue = eliminadoProp.GetValue(entity) as bool?;
                if (eliminadoValue == true)
                    throw new Backend.Exceptions.NotFoundException($"Entidad de tipo {typeof(T).Name} con ID {id} no encontrada (eliminada lógicamente).");
            }
            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new Backend.Exceptions.ValidationException("La entidad no puede ser nula.");
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new Backend.Exceptions.ValidationException("La entidad no puede ser nula.");
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
            if (id <= 0)
                throw new Backend.Exceptions.ValidationException("El ID debe ser mayor a cero.");
            var entity = await GetByIdAsync(id);
            // Si la entidad tiene la propiedad Eliminado, hacer borrado lógico
            var eliminadoProp = typeof(T).GetProperty("Eliminado");
            var fechaEliminacionProp = typeof(T).GetProperty("FechaEliminacion");
            if (eliminadoProp != null && eliminadoProp.PropertyType == typeof(bool?))
            {
                eliminadoProp.SetValue(entity, true);
                if (fechaEliminacionProp != null && fechaEliminacionProp.PropertyType == typeof(DateTime?))
                {
                    fechaEliminacionProp.SetValue(entity, DateTime.Now);
                }
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
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