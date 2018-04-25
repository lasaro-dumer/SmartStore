using System.Collections.Generic;
using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
    }
}
