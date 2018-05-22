using System.Collections.Generic;
using System.Linq;
using SmartStore.Data.Entities;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data.Repositories
{
    public class StockRepository : BaseRepository, IStockRepository
    {
        public StockRepository(SmartStoreDbContext context) : base(context)
        {
        }

        public IEnumerable<StockMovementType> GetMovementTypes()
        {
            return _context.StockMovementTypes.ToList();
        }
    }
}
