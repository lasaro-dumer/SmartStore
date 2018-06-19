using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Entities
{
    public class CartItem
    {
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        [Required]
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}