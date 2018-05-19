using System;
using System.Collections.Generic;
using System.Linq;
using SmartStore.Data.Entities;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data.Repositories
{
    public class ProductsRepository : BaseRepository, IProductsRepository
    {
        public ProductsRepository(SmartStoreDbContext context)
            : base(context)
        {

        }

        public Product GetProductById(int id)
        {
            return _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public IEnumerable<Product> GetProductsWithStock(string name = null,
                                                         string description = null,
                                                         decimal? minSellingPrice = null, 
                                                         decimal? maxSellingPrice = null, 
                                                         int? minStock = null,
                                                         int? maxStock = null)
        {
            var products = (from p in _context.Products
                            where (name == null || p.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase)) &&
                                  (description == null || p.Description.Contains(description)) &&
                                  (!minSellingPrice.HasValue || p.SellingPrice >= minSellingPrice) &&
                                  (!maxSellingPrice.HasValue || p.SellingPrice <= maxSellingPrice)
                            select p).ToList();

            return products;
        }
    }
}
