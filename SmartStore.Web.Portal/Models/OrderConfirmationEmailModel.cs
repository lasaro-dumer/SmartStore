using SmartStore.Data.Models;

namespace SmartStore.Web.Portal.Models
{
    public class OrderConfirmationEmailModel
    {
        public string Name { get; internal set; }
        public string PurchaseDetailsUrl { get; internal set; }
        public OrderModel Order { get; internal set; }
    }
}
