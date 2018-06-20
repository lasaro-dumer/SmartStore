using System;
using System.Collections.Generic;

namespace SmartStore.Web.Portal.Models
{
    public class CartItemModel : IEquatable<CartItemModel>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as CartItemModel);
        }

        public bool Equals(CartItemModel other)
        {
            return other != null &&
                   ProductId == other.ProductId;
        }

        public override int GetHashCode()
        {
            return -661295095 + ProductId.GetHashCode();
        }

        public static bool operator ==(CartItemModel model1, CartItemModel model2)
        {
            return EqualityComparer<CartItemModel>.Default.Equals(model1, model2);
        }

        public static bool operator !=(CartItemModel model1, CartItemModel model2)
        {
            return !(model1 == model2);
        }
    }
}