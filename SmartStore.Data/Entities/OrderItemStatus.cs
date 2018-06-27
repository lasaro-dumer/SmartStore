using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Entities
{
    public class OrderItemStatus
    {
        public const string _WaitingStock = "Waiting Stock";
        public const string _Packing = "Packing";
        public const string _Packed = "Packed";

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
