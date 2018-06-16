using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }
        [Display(Name = "Stock Balance")]
        public int StockBalance { get; set; }
        [Display(Name = "Last Stock Movement Date")]
        public DateTime LastStockMovementDate { get; set; }
        public string[] Tags { get; set; }
    }
}
