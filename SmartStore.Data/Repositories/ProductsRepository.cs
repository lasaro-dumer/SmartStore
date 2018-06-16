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
                            .Include("ProductTags.Tag")
                            .FirstOrDefault();
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products
                            .Include("ProductTags.Tag")
                            .ToList();
        }

        public IEnumerable<StockMovement> GetProductsWithStock(string name = null,
                                                               string description = null,
                                                               decimal? minSellingPrice = null,
                                                               decimal? maxSellingPrice = null,
                                                               int? minStockBalance = null,
                                                               int? maxStockBalance = null,
                                                               int? recordsToReturn = null,
                                                               string[] tags = null)
        {
            var productsQuery = _context.Products
                                        .Include("ProductTags.Tag")
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                productsQuery = productsQuery.Where(s => s.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase));

            if (!string.IsNullOrEmpty(description))
                productsQuery = productsQuery.Where(s => s.Description.Contains(description));

            if (minSellingPrice.HasValue)
                productsQuery = productsQuery.Where(s => s.SellingPrice >= minSellingPrice);

            if (maxSellingPrice.HasValue)
                productsQuery = productsQuery.Where(s => s.SellingPrice <= maxSellingPrice);

            var query = _context.StockMoviments
                                .Include(s => s.Product)
                                .AsQueryable();

            List<StockMovement> productsStock = new List<StockMovement>();

            var products = productsQuery.ToList();

            if (tags != null && tags.Length > 0)
                products = products.Where(p => !tags.Except(p.Tags.Select(t => t.Name)).Any()).ToList();

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

        public void FillTags(Product product)
        {
            var tagString = product.Tags.Select(t => t.Name).ToArray();
            var existingTags = _context.Tags.Where(t => tagString.Contains(t.Name)).ToList();

            product.FillTags(existingTags);
        }

        public IEnumerable<Tag> GetExistingTags()
        {
            return _context.Tags.ToList();
        }

        public IEnumerable<Product> GetProducts(string name, string description, decimal? minSellingPrice, decimal? maxSellingPrice, int? productsToList, string[] tags)
        {
            var productsQuery = _context.Products
                                           .Include("ProductTags.Tag")
                                           .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                productsQuery = productsQuery.Where(s => s.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase));

            if (!string.IsNullOrEmpty(description))
                productsQuery = productsQuery.Where(s => s.Description.Contains(description));

            if (minSellingPrice.HasValue)
                productsQuery = productsQuery.Where(s => s.SellingPrice >= minSellingPrice);

            if (maxSellingPrice.HasValue)
                productsQuery = productsQuery.Where(s => s.SellingPrice <= maxSellingPrice);

            var products = productsQuery.ToList();

            if (tags != null && tags.Length > 0)
                products = products.Where(p => !tags.Except(p.Tags.Select(t => t.Name)).Any()).ToList();

            if (productsToList.HasValue)
                products = products.Take(productsToList.Value).ToList();

            return products;
        }
    }
}
