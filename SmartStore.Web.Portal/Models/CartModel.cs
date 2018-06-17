using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStore.Web.Portal.Models
{
    public class CartModel
    {
        public CartModel()
        {
            Products = new CartProductModel[] { };
        }

        public CartProductModel[] Products { get; set; }
    }
}
