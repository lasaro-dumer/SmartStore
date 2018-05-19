using System.Collections.Generic;
using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IProductsRepository : IBaseRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductsWithStock(string name = null, string description = null, decimal? minSellingPrice = null, decimal? maxSellingPrice = null, int? minStock = null, int? maxStock = null);
        Product GetProductById(int id);
    }
}
