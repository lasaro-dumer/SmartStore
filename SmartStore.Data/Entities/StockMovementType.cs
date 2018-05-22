using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Entities
{
    public class StockMovementType
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
