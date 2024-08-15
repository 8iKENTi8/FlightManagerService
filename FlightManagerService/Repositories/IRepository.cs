using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightManagerService.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(); // Получить все записи
        Task<bool> ReplaceAllAsync(IEnumerable<T> entities); // Полная замена всех записей
        Task AddAsync(IEnumerable<T> entities); // Добавление новых записей
    }
}
