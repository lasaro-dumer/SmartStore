using System;
using System.Linq;

namespace SmartStore.Web.Portal.Models
{
    public class CartModel
    {
        public CartModel()
        {
            CartItems = new CartItemModel[] { };
            LastUpdated = DateTime.Now;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string UnauthenticatedUserId { get; set; }
        public CartItemModel[] CartItems { get; set; }
        public DateTime LastUpdated { get; set; }

        internal bool Merge(CartModel otherCart)
        {
            if (otherCart != null && Id != otherCart.Id)
            {
                var existingItems = otherCart.CartItems.Intersect(CartItems);
                var newItems = otherCart.CartItems.Except(existingItems);

                for (int i = 0; i < CartItems.Length; i++)
                {
                    int? moreQuantity = (existingItems.FirstOrDefault(ci => ci.ProductId == CartItems[i].ProductId)?.Quantity);
                    if (moreQuantity.HasValue)
                        CartItems[i].Quantity += moreQuantity.Value;
                }

                CartItems = CartItems.Concat(newItems).ToArray();

                return true;
            }

            return false;
        }
    }
}
