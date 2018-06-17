using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;
using SmartStore.Web.Portal.Models;
using SmartStore.Web.Portal.Utility;

namespace SmartStore.Web.Portal.Controllers
{
    public class CartController : Controller
    {
        private ILogger<CartController> _logger;
        private readonly IProductsRepository _productsRepo;

        public CartController(ILogger<CartController> logger,
                              IProductsRepository productsRepo)
        {
            _logger = logger;
            _productsRepo = productsRepo;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Index()
        {
            CartModel cart = new CartModel();

            try
            {
                cart = HttpContext.Session.Get<CartModel>(Utility.SessionExtensions.SessionCart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return View(cart);
        }

        [HttpPost, AllowAnonymous]
        public IActionResult AddProductToCart([FromBody] ProductModel product)
        {
            try
            {
                var sessionCart = HttpContext.Session.Get<CartModel>(Utility.SessionExtensions.SessionCart);
                if (!sessionCart.Products.Any(p => p.ProductId == product.Id))
                {
                    var prod = _productsRepo.GetProductById(product.Id);

                    CartProductModel newCartProduct = new CartProductModel()
                    {
                        ProductId = prod.Id,
                        ProductName = prod.Name,
                        UnitPrice = prod.SellingPrice,
                        Quantity = 1
                    };

                    sessionCart.Products = sessionCart.Products.Concat(new[] { newCartProduct }).ToArray();

                    HttpContext.Session.Set(Utility.SessionExtensions.SessionCart, sessionCart);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost, AllowAnonymous]
        public IActionResult SaveCartToSession([FromBody] CartModel cart)
        {
            try
            {
                HttpContext.Session.Set(Utility.SessionExtensions.SessionCart, cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            return Ok();
        }
    }
}