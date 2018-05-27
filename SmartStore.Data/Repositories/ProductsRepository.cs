using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<StockMovement> GetProductsWithStock(string name = null,
                                                         string description = null,
                                                         decimal? minSellingPrice = null,
                                                         decimal? maxSellingPrice = null,
                                                         int? minStockBalance = null,
                                                         int? maxStockBalance = null,
                                                         int? recordsToReturn = null)
        {

            var query = _context.StockMoviments
                                .Include(s => s.Product)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(s => s.Product.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase));

            if (!string.IsNullOrEmpty(description))
                query = query.Where(s => s.Product.Description.Contains(description));

            if (minSellingPrice.HasValue)
                query = query.Where(s => s.Product.SellingPrice >= minSellingPrice);

            if (maxSellingPrice.HasValue)
                query = query.Where(s => s.Product.SellingPrice <= maxSellingPrice);
            
            List<StockMovement> productsStock = new List<StockMovement>();

            var products = query.Select(s => s.Product).Distinct().ToList();

            foreach (var product in products)
            {
                productsStock.Add(query.Where(s => s.Product.Id == product.Id)
                                        .OrderByDescending(s => s.Date)
                                        .FirstOrDefault());
            }

            if (minStockBalance.HasValue)
                productsStock = productsStock.Where(s => s.Balance >= minStockBalance).ToList();

            if (maxStockBalance.HasValue)
                productsStock = productsStock.Where(s => s.Balance <= maxStockBalance).ToList();

            if (recordsToReturn.HasValue)
                productsStock = productsStock.Take(recordsToReturn.Value).ToList();

            return productsStock.ToList();
        }
    }
}
