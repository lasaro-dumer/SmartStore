using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStore.Data.Entities;

namespace SmartStore.Data
{
    public class SmartStoreRepository : ISmartStoreRepository
    {
        private SmartStoreDbContext _context;

        public SmartStoreRepository(SmartStoreDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
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

        public async Task<bool> SaveAllAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
