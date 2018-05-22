using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStore.Data.Models;

namespace SmartStore.Web.Portal.Models
{
    public class StockSearchFilter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? MinSellingPrice { get; set; }
        public decimal? MaxSellingPrice { get; set; }
        public int? MinStockBalance { get; set; }
        public int? MaxStockBalance { get; set; }
    }
}
