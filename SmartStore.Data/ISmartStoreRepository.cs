using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.Data.Entities;

namespace SmartStore.Data
{
    public interface ISmartStoreRepository
    {
        // Basic DB Operations
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();

        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
    }
}
