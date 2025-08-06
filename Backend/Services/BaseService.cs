using AutoMapper;
using Backend.Interfaces;
using Backend.Exceptions;
using System.Reflection;

namespace Backend.Services
{
public abstract class BaseService<TEntity, TDto, TCreateDto> : IService<TEntity, TDto, TCreateDto>
    where TEntity : class
    where TDto : class
    where TCreateDto : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

    protected BaseService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

    public virtual async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"Entidad con ID {id} no encontrada");
            return _mapper.Map<TDto>(entity);
        }

    public virtual async Task<TDto> CreateAsync(TCreateDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        var result = await _repository.AddAsync(entity);
        return _mapper.Map<TDto>(result);
    }

    public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            if (dto == null)
                throw new Exceptions.AppException("El objeto recibido no puede ser nulo.");
            var id = GetIdFromDto(dto);
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new NotFoundException($"Entidad con ID {id} no encontrada");
            // Create a new entity from the DTO and preserve the ID
            var updatedEntity = _mapper.Map<TEntity>(dto);
            // Use reflection to set the ID property if it exists
            var idProperty = GetIdProperty(updatedEntity);
            if (idProperty != null && idProperty.CanWrite)
            {
                idProperty.SetValue(updatedEntity, id);
            }
            var result = await _repository.UpdateAsync(updatedEntity);
            return _mapper.Map<TDto>(result);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

    protected abstract int GetIdFromDto(TDto dto);

        private PropertyInfo? GetIdProperty(TEntity entity)
        {
            var type = typeof(TEntity);
            
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