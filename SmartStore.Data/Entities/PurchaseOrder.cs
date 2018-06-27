using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Entities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        [Required]
        public UserEntity User { get; set; }
        [Required]
        public string CreditCarNumber { get; set; }
        [Required]
        public string CreditCarCompany { get; set; }
        [Required]
        public DateTime Creation { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        public IEnumerable<PurchaseOrderItem> OrderItems { get; set; }

        public PurchaseOrder()
        {
            OrderItems = new List<PurchaseOrderItem>();
        }
    }
}
