using AutoMapper;
using Backend.Interfaces;
using Backend.Exceptions;

namespace Backend.Services
{
    public abstract class BaseService<TEntity, TDto> : IService<TEntity, TDto> 
        where TEntity : class 
        where TDto : class
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

        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var result = await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(result);
        }

        public virtual async Task<TDto> UpdateAsync(int id, TDto dto)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new NotFoundException($"Entidad con ID {id} no encontrada");
            
            // Create a new entity from the DTO and preserve the ID
            var updatedEntity = _mapper.Map<TEntity>(dto);
            
            // Use reflection to set the ID property if it exists
            var idProperty = typeof(TEntity).GetProperty("Id");
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
    }
}