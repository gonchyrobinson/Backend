namespace Backend.Interfaces
{
    public interface IService<TEntity, TDto, TCreateDto> where TEntity : class where TDto : class where TCreateDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(int id);
        Task<TDto> CreateAsync(TCreateDto dto);
        Task<TDto> UpdateAsync(TDto dto);
        Task<bool> DeleteAsync(int id);
    }
}