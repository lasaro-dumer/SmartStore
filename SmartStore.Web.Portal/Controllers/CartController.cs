using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;
using SmartStore.Web.Portal.Models;
using SmartStore.Web.Portal.Utility;

namespace SmartStore.Web.Portal.Controllers
{
    public class CartController : Controller
    {
        private ILogger<CartController> _logger;
        private IProductsRepository _productsRepo;
        private IUsersRepository _usersRepo;
        private IShoppingRepository _shoppingRepo;
        private IMapper _mapper;

        public CartController(ILogger<CartController> logger,
                              IProductsRepository productsRepo,
                              IUsersRepository usersRepo,
                              IShoppingRepository shoppingRepo,
                              IMapper mapper)
        {
            _logger = logger;
            _productsRepo = productsRepo;
            _usersRepo = usersRepo;
            _shoppingRepo = shoppingRepo;
            _mapper = mapper;
        }

        private CartModel GetCartFromSession()
        {
            CartModel sessionCart = HttpContext.Session.Get<CartModel>(Utility.SessionExtensions.SessionCart);

            if (sessionCart == null)
            {
                string userId = null;
                string cookieUserId = null;

                if (User.Identity.IsAuthenticated)
                {
                    string userName = User.Identity.Name;

                    UserEntity ue = _usersRepo.GetUserByUsername(userName);
                    userId = ue.Id;
                }
                else
                {
                    cookieUserId = HttpContext.Request.Cookies[BaseController.CookieUserId];
                }

                ShoppingCart shoppingCart = _shoppingRepo.GetShoppingCartFromUser(userId, cookieUserId);
                if (shoppingCart != null)
                {
                    sessionCart = _mapper.Map<CartModel>(shoppingCart);
                }
                else
                {
                    sessionCart = new CartModel();
                }
            }

            HttpContext.Session.Set(Utility.SessionExtensions.SessionCart, sessionCart);

            return sessionCart;
        }

        private void SaveCartToDatabase(CartModel cart)
        {
            string userId = null;
            string userName = null;
            string cookieUserId = null;

            if (User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;

                UserEntity ue = _usersRepo.GetUserByUsername(userName);
                userId = ue.Id;
            }
            else
            {
                cookieUserId = HttpContext.Request.Cookies[BaseController.CookieUserId];
            }

            cart.UserId = userId;
            cart.UnauthenticatedUserId = cookieUserId;

            ShoppingCart shoppingCart = _mapper.Map<ShoppingCart>(cart);

            shoppingCart = _shoppingRepo.SaveCart(shoppingCart);
            cart.Id = shoppingCart.Id;

            HttpContext.Session.Set(Utility.SessionExtensions.SessionCart, cart);
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Index()
        {
            CartModel cart = new CartModel();

            try
            {
                cart = GetCartFromSession();
                foreach (CartItemModel cartProduct in cart.CartItems)
                {
                    var prod = _productsRepo.GetProductById(cartProduct.ProductId);
                    cartProduct.ProductName = prod.Name;
                    cartProduct.UnitPrice = prod.SellingPrice;
                }
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
                CartModel sessionCart = GetCartFromSession();
                if (!sessionCart.CartItems.Any(p => p.ProductId == product.Id))
                {
                    var prod = _productsRepo.GetProductById(product.Id);

                    CartItemModel newCartProduct = new CartItemModel()
                    {
                        ProductId = prod.Id,
                        ProductName = prod.Name,
                        UnitPrice = prod.SellingPrice,
                        Quantity = 1
                    };

                    sessionCart.CartItems = sessionCart.CartItems.Concat(new[] { newCartProduct }).ToArray();
                    sessionCart.LastUpdated = DateTime.Now;
                    SaveCartToDatabase(sessionCart);
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
                CartModel sessionCart = GetCartFromSession();

                sessionCart.LastUpdated = DateTime.Now;
                sessionCart.CartItems = cart.CartItems;

                SaveCartToDatabase(sessionCart);
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