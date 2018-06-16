using System.Collections.Generic;
using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IProductsRepository : IBaseRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<StockMovement> GetProductsWithStock(string name = null,
                                                        string description = null,
                                                        decimal? minSellingPrice = null,
                                                        decimal? maxSellingPrice = null,
                                                        int? minStockBalance = null,
                                                        int? maxStockBalance = null,
                                                        int? recordsToReturn = null,
                                                        string[] tags = null);
        Product GetProductById(int id);
        void FillTags(Product product);
        IEnumerable<Tag> GetExistingTags();
        IEnumerable<Product> GetProducts(string name = null,
                                         string description = null,
                                         decimal? minSellingPrice = null,
                                         decimal? maxSellingPrice = null,
                                         int? productsToList = null,
                                         string[] tags = null);
    }
}
