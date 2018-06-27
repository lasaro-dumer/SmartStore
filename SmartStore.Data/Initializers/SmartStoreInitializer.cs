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
        private IShoppingRepository _shoppingRepo;

        public SmartStoreInitializer(IStockRepository stockRepository,
                                     IShoppingRepository shoppingRepository)
        {
            _stockRepo = stockRepository;
            _shoppingRepo = shoppingRepository;
        }

        public async Task<bool> Seed()
        {
            bool movementTypeSaved = await SaveMovementTypes();
            bool orderItemStatusSaved = await SaveOrderItemSatuses();
            bool orderStatusSaved = await SaveOrderStatuses();

            return movementTypeSaved && orderItemStatusSaved && orderStatusSaved;
        }

        private async Task<bool> SaveOrderStatuses()
        {
            List<OrderStatus> defaultOrderStatuses = new List<OrderStatus>()
            {
                new OrderStatus(){ Name = OrderStatus._WaitingStock},
                new OrderStatus(){ Name = OrderStatus._WaitingPayment},
                new OrderStatus(){ Name = OrderStatus._Packing},
                new OrderStatus(){ Name = OrderStatus._Delivering},
                new OrderStatus(){ Name = OrderStatus._Delivered}
            };

            IEnumerable<OrderStatus> existingOrderStatuses = _shoppingRepo.GetOrderStatuses();

            foreach (OrderStatus orderStatus in defaultOrderStatuses)
            {
                if (!existingOrderStatuses.Any(m => m.Name == orderStatus.Name))
                    _shoppingRepo.Add(orderStatus);
            }

            return await _shoppingRepo.SaveAllAsync();
        }

        private async Task<bool> SaveOrderItemSatuses()
        {
            List<OrderItemStatus> defaultOrderItemStatuses = new List<OrderItemStatus>()
            {
                new OrderItemStatus(){ Name = OrderItemStatus._WaitingStock},
                new OrderItemStatus(){ Name = OrderItemStatus._Packing},
                new OrderItemStatus(){ Name = OrderItemStatus._Packed}
            };

            IEnumerable<OrderItemStatus> existingOrderItemStatuses = _shoppingRepo.GetOrderItemStatuses();

            foreach (OrderItemStatus orderItemStatus in defaultOrderItemStatuses)
            {
                if (!existingOrderItemStatuses.Any(m => m.Name == orderItemStatus.Name))
                    _shoppingRepo.Add(orderItemStatus);
            }

            return await _shoppingRepo.SaveAllAsync();
        }

        private async Task<bool> SaveMovementTypes()
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
