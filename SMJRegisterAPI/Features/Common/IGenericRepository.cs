namespace SMJRegisterAPI.Features.Common;

public interface IGenericRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity, int id);
    Task DeleteAsync(T entity);
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
}