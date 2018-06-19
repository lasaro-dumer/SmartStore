using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartStore.Data.Entities;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data.Repositories
{
    public class ShoppingRepository : BaseRepository, IShoppingRepository
    {
        public ShoppingRepository(SmartStoreDbContext context)
            : base(context)
        {
        }

        public ShoppingCart SaveCart(ShoppingCart shoppingCart)
        {
            for (int i = 0; i < shoppingCart.CartItems.Count; i++)
            {
                int prodId = shoppingCart.CartItems[i].Product.Id;
                shoppingCart.CartItems[i].Product = _context.Products.FirstOrDefault(p => p.Id == prodId);
            }

            if (shoppingCart.Id != 0)
            {
                ShoppingCart storedCart = _context.ShoppingCarts
                                                  .Include(s => s.CartItems)
                                                  .FirstOrDefault(s => s.Id == shoppingCart.Id);
                //storedCart.CartItems = shoppingCart.CartItems;
                var cartItemsToRemove = storedCart.CartItems
                                             .Where(ci => !shoppingCart.CartItems
                                                                       .Any(ci2 => ci.ProductId == ci2.ProductId))
                                             .ToList();
                foreach (var cartItem in cartItemsToRemove)
                {
                    storedCart.CartItems.Remove(cartItem);
                }
                foreach (var cartItem in shoppingCart.CartItems)
                {
                    var ci = storedCart.CartItems
                                       .FirstOrDefault(c => c.ProductId == cartItem.ProductId);
                    if (ci == null)
                    {
                        storedCart.CartItems.Add(cartItem);
                    }
                    else
                    {
                        ci.Quantity = cartItem.Quantity;
                    }
                }

                storedCart.LastUpdated = shoppingCart.LastUpdated;
                storedCart.UserId = shoppingCart.UserId;
                storedCart.UnauthenticatedUserId = shoppingCart.UnauthenticatedUserId;
            }
            else
            {
                _context.Add(shoppingCart);
            }

            _context.SaveChanges();

            return shoppingCart;
        }

        public ShoppingCart GetShoppingCartFromUser(string userId, string unauthenticatedUserId)
        {
            ShoppingCart shoppingCart = null;
            if (!string.IsNullOrEmpty(userId))
            {
                shoppingCart = _context.ShoppingCarts
                                        .Include(s => s.CartItems)
                                            .ThenInclude(c => c.Product)
                                        .FirstOrDefault(s => s.UserId == userId);
            }
            else if (!string.IsNullOrEmpty(unauthenticatedUserId))
            {
                shoppingCart = _context.ShoppingCarts
                                        .Include(s => s.CartItems)
                                            .ThenInclude(c => c.Product)
                                        .FirstOrDefault(s => s.UnauthenticatedUserId == unauthenticatedUserId);
            }
            return shoppingCart;
        }

        public void DeleteCart(ShoppingCart shoppingCart)
        {
            Delete(shoppingCart);

            _context.SaveChanges();
        }
    }
}
