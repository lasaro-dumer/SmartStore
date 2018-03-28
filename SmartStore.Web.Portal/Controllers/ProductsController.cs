using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Domain.Interfaces.Repositories;
using SmartStore.Domain.Models;

namespace SmartStore.Web.Portal.Controllers
{
    public class ProductsController : Controller
    {
        private IProductsRepository _productsRepo { get; }

        public ProductsController(IProductsRepository productsRepo)
        {
            _productsRepo = productsRepo;
        }

        public IActionResult Index()
        {
            List<ProductModel> products = new List<ProductModel>();

            products = _productsRepo.GetProducts();

            return View(products);
        }
    }
}