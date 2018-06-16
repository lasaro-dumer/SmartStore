using System;
using System.Linq;
using System.Threading.Tasks;
using SmartStore.Data.Entities;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        protected SmartStoreDbContext _context;

        public BaseRepository(SmartStoreDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);

            if (entity is Product product)
            {
                StockMovement firstMovement = new StockMovement()
                {
                    Amount = 0,
                    Product = product,
                    Balance = 0,
                    Date = DateTime.Now,
                    MovementType = _context.StockMovementTypes.Where(m => m.Name == "Initial balance").FirstOrDefault()
                };

                _context.Add(firstMovement);
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<bool> SaveAllAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
