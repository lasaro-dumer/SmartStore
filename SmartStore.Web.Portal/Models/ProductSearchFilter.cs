using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStore.Web.Portal.Models
{
    public class ProductSearchFilter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? MinSellingPrice { get; set; }
        public decimal? MaxSellingPrice { get; set; }
        public int? ProductsToList { get; set; }
        public string[] Tags { get; set; }
    }
}
