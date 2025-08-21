namespace Backend.Interfaces.Services
{
    public interface IService<TEntity, TDto, TUpdateDto, TCreateDto> 
        where TEntity : class 
        where TDto : class 
        where TUpdateDto : class 
        where TCreateDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(int id);
        Task<TDto> CreateAsync(TCreateDto dto);
        Task<TDto> UpdateAsync(TUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}