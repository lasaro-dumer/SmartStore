using Microsoft.AspNetCore.Mvc;
using SmartStore.Domain.Models;

namespace SmartStore.Web.Portal.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Details()
        {
            var prod = new ProductModel()
            {
                Name = "Name products",
                Description = "A description",
                SellingPrice = 1.89m
            };
            return View(prod);
        }
    }
}