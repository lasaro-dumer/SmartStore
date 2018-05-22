using System.Collections.Generic;
using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IStockRepository : IBaseRepository
    {
        IEnumerable<StockMovementType> GetMovementTypes();
    }
}
