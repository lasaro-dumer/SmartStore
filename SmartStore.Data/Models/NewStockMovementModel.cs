using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartStore.Data.Models
{
    public class NewStockMovementModel
    {
        [Required]
        [Display(Name = "Movement Type")]
        public int StockMovementTypeId { get; set; }
        public List<SelectListItem> StockMovementTypes { get; set; }
        [Required]
        public ProductModel Product { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
