using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStore.Data.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UnauthenticatedUserId { get; set; }
        [Required]
        public DateTime LastUpdated { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
