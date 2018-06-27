using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Entities
{
    public class OrderStatus
    {
        public const string _WaitingStock = "Waiting Stock";
        public const string _WaitingPayment = "Waiting Payment";
        public const string _Packing = "Packing";
        public const string _Delivering = "Delivering";
        public const string _Delivered = "Delivered";

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
