using Microsoft.AspNetCore.Mvc;

namespace SmartStore.Web.Portal.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}