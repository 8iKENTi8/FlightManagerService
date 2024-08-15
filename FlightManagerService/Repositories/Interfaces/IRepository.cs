public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> ReplaceAllAsync(IEnumerable<T> entities);
    Task<bool> AddAsync(IEnumerable<T> entities); 
}