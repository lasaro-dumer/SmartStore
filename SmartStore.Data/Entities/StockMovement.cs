using System;
using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Entities
{
    public class StockMovement
    {
        public int Id { get; set; }
        [Required]
        public StockMovementType MovementType { get; set; }
        [Required]
        public Product Product { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Balance { get; set; }
    }
}
