using Microsoft.AspNetCore.Mvc;

namespace SmartStore.Web.Portal.Controllers
{
    public class StockController : Controller
    {
        public IActionResult NewProduct()
        {
            return View();
        }

        public IActionResult Status()
        {
            return View();
        }
    }
}