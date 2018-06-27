using System.Collections.Generic;
using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IShoppingRepository : IBaseRepository
    {
        ShoppingCart SaveCart(ShoppingCart shoppingCart);
        ShoppingCart GetShoppingCartFromUser(string userId, string unauthenticatedUserId);
        void DeleteCart(ShoppingCart shoppingCart);
        IEnumerable<OrderItemStatus> GetOrderItemStatuses();
        IEnumerable<OrderStatus> GetOrderStatuses();
        PurchaseOrder CreateClientPurchase(ShoppingCart shoppingCart);
        PurchaseOrder GetPurchaseOrderById(int id);
        IEnumerable<PurchaseOrder> GetPurchaseOrdersFromUser(string userName);
    }
}
