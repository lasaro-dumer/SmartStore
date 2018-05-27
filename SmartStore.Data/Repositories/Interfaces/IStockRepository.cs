using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IStockRepository : IBaseRepository
    {
        IEnumerable<StockMovementType> GetMovementTypes(bool onlyAvailableAtScreen = true);
        Task<bool> AddStockMovement(int productId, int movementTypeId, int amount);
    }
}
