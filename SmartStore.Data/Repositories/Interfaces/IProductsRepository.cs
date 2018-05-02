using System.Collections.Generic;
using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IProductsRepository : IBaseRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
    }
}
