using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Domain.Models;
using SmartStore.Web.Portal.Clients;

namespace SmartStore.Web.Portal.Controllers
{
    public class ProductsController : Controller
    {
        public ProductsClient ProductsClient { get; }

        public ProductsController(ProductsClient productsClient)
        {
            ProductsClient = productsClient;
        }

        public IActionResult Index()
        {
            List<ProductModel> products = new List<ProductModel>();

            products = ProductsClient.GetProducts();

            return View(products);
        }
    }
}