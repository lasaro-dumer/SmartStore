using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data.Repositories
{
    public class StockRepository : BaseRepository, IStockRepository
    {
        public StockRepository(SmartStoreDbContext context) : base(context)
        {
        }

        public IEnumerable<StockMovementType> GetMovementTypes(bool onlyAvailableAtScreen = true)
        {
            return _context.StockMovementTypes
                            .Where(t => (t.AvailableAtScreen || !onlyAvailableAtScreen))
                            .ToList();
        }

        public StockMovement LastStockMovement(int productId)
        {
            return _context.StockMoviments
                            .Where(s => s.Product.Id == productId)
                            .OrderByDescending(s => s.Date)
                            .FirstOrDefault();
        }

        public async Task<bool> AddStockMovement(int productId, int movementTypeId, int amount)
        {
            StockMovement stockMovement = new StockMovement()
            {
                Product = _context.Products.SingleOrDefault(p => p.Id == productId),
                MovementType = _context.StockMovementTypes.SingleOrDefault(t => t.Id == movementTypeId),
                Amount = amount,
                Date = DateTime.Now
            };

            if (stockMovement.Product != null && stockMovement.MovementType != null)
            {
                StockMovement lastStockMovement = LastStockMovement(productId);

                stockMovement.Balance = lastStockMovement.Balance + stockMovement.Amount;

                _context.StockMoviments.Add(stockMovement);

                return await SaveAllAsync();
            }

            return false;
        }
    }
}
