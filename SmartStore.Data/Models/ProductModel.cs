﻿using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Models
{
    public class ProductModel
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }
    }
}
