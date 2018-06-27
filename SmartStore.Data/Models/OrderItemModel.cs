namespace SmartStore.Data.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public ProductModel Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public OrderModel Order { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get => UnitPrice * Quantity; }
    }
}
