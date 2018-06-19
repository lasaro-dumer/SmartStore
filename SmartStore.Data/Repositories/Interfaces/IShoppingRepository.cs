using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IShoppingRepository : IBaseRepository
    {
        ShoppingCart SaveCart(ShoppingCart shoppingCart);
        ShoppingCart GetShoppingCartFromUser(string userId, string unauthenticatedUserId);
        void DeleteCart(ShoppingCart shoppingCart);
    }
}
