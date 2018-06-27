using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartStore.Data.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        [Display(Name = "Credit Car Number")]
        public string CreditCarNumber { get; set; }
        [Display(Name = "Credit Car Company")]
        public string CreditCarCompany { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime Creation { get; set; }
        [Display(Name = "Last Update Date")]
        public DateTime Updated { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; }
        [Display(Name = "Total")]
        public decimal Total { get => OrderItems.Sum(i => i.TotalPrice); }
    }
}
