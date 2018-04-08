using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
