using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStore.Data.Entities;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data.Initializers
{
    public class SmartStoreInitializer
    {
        private IStockRepository _stockRepo;

        public SmartStoreInitializer(IStockRepository stockRepository)
        {
            _stockRepo = stockRepository;
        }

        public async Task<bool> Seed()
        {
            List<StockMovementType> defaultMovementTypes = new List<StockMovementType>()
            {
                new StockMovementType() { Name = "Initial balance" },
                new StockMovementType() { Name = "Normal" },
                new StockMovementType() { Name = "Supplier purchase" },
                new StockMovementType() { Name = "User purchase" },
                new StockMovementType() { Name = "Warehouse review" }
            };

            IEnumerable<StockMovementType> existingMovementTypes = _stockRepo.GetMovementTypes();

            foreach (StockMovementType movementType in defaultMovementTypes)
            {
                if (!existingMovementTypes.Any(m => m.Name == movementType.Name))
                    _stockRepo.Add(movementType);
            }

            return await _stockRepo.SaveAllAsync();
        }
    }
}
