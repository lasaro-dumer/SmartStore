using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Entities
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }
        [Required]
        public Product Product { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public PurchaseOrder Order { get; set; }
        [Required]
        public OrderItemStatus Status { get; set; }
    }
}
