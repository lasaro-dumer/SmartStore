using System.Threading.Tasks;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IBaseRepository
    {
        // Basic DB Operations
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();
    }
}
