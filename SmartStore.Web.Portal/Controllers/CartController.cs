using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class CartController : BaseController
    {
        private ILogger<CartController> _logger;
        private IProductsRepository _productsRepo;
        private IUsersRepository _usersRepo;
        private IShoppingRepository _shoppingRepo;
        private IMapper _mapper;
        private EmailSender _emailSender;

        public CartController(ILogger<CartController> logger,
                              IProductsRepository productsRepo,
                              IUsersRepository usersRepo,
                              IShoppingRepository shoppingRepo,
                              IMapper mapper,
                              EmailSender emailSender)
        {
            _logger = logger;
            _productsRepo = productsRepo;
            _usersRepo = usersRepo;
            _shoppingRepo = shoppingRepo;
            _mapper = mapper;
            _emailSender = emailSender;
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

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Checkout([FromForm] CartModel cart)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    UserEntity ue = _usersRepo.GetUserById(cart.UserId);

                    if (!string.IsNullOrEmpty(ue.CreditCardNumber) && !string.IsNullOrEmpty(ue.CreditCardCompany))
                    {
                        PurchaseOrder purchaseOrder = _shoppingRepo.CreateClientPurchase(_mapper.Map<ShoppingCart>(GetCartFromSession()));

                        this.AddInformationMessage("Purchase order created");

                        HttpContext.Session.Remove(Utility.SessionExtensions.SessionCart);

                        if (ue.EmailConfirmed)
                        {
                            bool emailSent = await _emailSender.SendPurchaseCreatedEmailAsync(ue.Email,
                                                                                              ue.FirstName,
                                                                                              ue.LastName,
                                                                                              _mapper.Map<OrderModel>(purchaseOrder));
                            if (emailSent)
                            {
                                this.AddInformationMessage($"Email confirming purchase order sent to '{ue.Email}'");
                            }
                            else
                            {
                                this.AddErrorMessage($"Failed to send email confirming purchase order to '{ue.Email}'");
                            }
                        }
                        else
                        {
                            this.AddWarningMessage($"Your email '{ue.Email}' isn't confirmed, so no email will be sent about the purchase order.");
                        }

                        return RedirectToAction("Details", "Purchase", new { id = purchaseOrder.Id });
                    }
                    else
                    {
                        this.AddInformationMessage("You need to fill your billing information before checkout");
                        return RedirectToAction("Billing", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account", new { ReturnUrl = "/Cart" });
                }
            }
            catch (Exception ex)
            {
                string errorGuid = Guid.NewGuid().ToString();
                this.AddErrorMessage($"An error ocurred while processing your request. Provide the Id: '{errorGuid}' for traceability");
                _logger.LogError(ex.Message, new { errorGuid, ex });
            }

            return RedirectToAction("Index");
        }
    }
}