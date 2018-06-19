using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;
using SmartStore.Web.Portal.Models;
using SmartStore.Web.Portal.Utility;

namespace SmartStore.Web.Portal.Controllers
{
    public class AccountController : BaseController
    {
        private SignInManager<UserEntity> _signInMgr;
        private UserManager<UserEntity> _userMgr;
        private IUsersRepository _usersRepo;
        private IMapper _mapper;
        private ILogger<AccountController> _logger;
        private readonly EmailSender _emailSender;
        private IShoppingRepository _shoppingRepo;

        public AccountController(SignInManager<UserEntity> signInMgr,
                                UserManager<UserEntity> userMgr,
                                IUsersRepository usersRepo,
                                IMapper mapper,
                                ILogger<AccountController> logger,
                                EmailSender emailSender,
                                IProductsRepository productsRepo,
                                IShoppingRepository shoppingRepo)
        {
            _signInMgr = signInMgr;
            _userMgr = userMgr;
            _usersRepo = usersRepo;
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
            _shoppingRepo = shoppingRepo;
        }

        [HttpGet, Authorize]
        public IActionResult Details()
        {
            var user = _usersRepo.GetUserByUsername(User.Identity.Name);

            return View(_mapper.Map<UserDetails>(user));
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]CredentialModel credential)
        {
            try
            {
                var result = await _signInMgr.PasswordSignInAsync(credential.UserName, credential.Password, false, false);

                if (result.Succeeded)
                {
                    CartModel cartFromDb = null;
                    CartModel sessionCart = HttpContext.Session.Get<CartModel>(Utility.SessionExtensions.SessionCart);

                    if (sessionCart == null)
                    {
                        sessionCart = new CartModel();
                    }

                    UserEntity ue = _usersRepo.GetUserByUsername(credential.UserName);

                    ShoppingCart shoppingCart = _shoppingRepo.GetShoppingCartFromUser(ue.Id, null);

                    if (shoppingCart != null)
                    {
                        cartFromDb = _mapper.Map<CartModel>(shoppingCart);

                        if (cartFromDb != null)
                        {
                            if (sessionCart.Merge(cartFromDb))
                            {
                                _shoppingRepo.DeleteCart(shoppingCart);
                                sessionCart.UserId = ue.Id;
                                sessionCart.UnauthenticatedUserId = null;
                                _shoppingRepo.SaveCart(_mapper.Map<ShoppingCart>(sessionCart));
                            }
                        }
                    }

                    HttpContext.Session.Set(Utility.SessionExtensions.SessionCart, sessionCart);

                    HttpContext.Response.Cookies.Delete(CookieUserId);

                    if (!string.IsNullOrEmpty(credential.ReturnUrl))
                        return Redirect(credential.ReturnUrl);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    this.AddErrorMessage("Failed to Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown while logging in: {ex}");
            }

            return View();// BadRequest("Failed to login");
        }

        [HttpGet, Authorize]
        public IActionResult Logout()
        {
            _signInMgr.SignOutAsync();
            HttpContext.Session.Remove(Utility.SessionExtensions.SessionCart);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async System.Threading.Tasks.Task<IActionResult> Register(NewUserModel user)
        {
            if (ModelState.IsValid)
            {
                UserEntity ue = _mapper.Map<UserEntity>(user);

                ue.EmailConfirmed = false;
                ue.EmailConfirmationToken = Guid.NewGuid().ToString();
                ue.EmailConfirmationExpiration = DateTime.Now.AddHours(24);

                IdentityResult result = await _userMgr.CreateAsync(ue, user.Password);
                if (result.Succeeded)
                {
                    await _userMgr.AddToRoleAsync(ue, "Client");

                    bool emailSent = await _emailSender.SendRegisterConfirmationEmailAsync(ue.Email,
                                                                                          ue.FirstName,
                                                                                          ue.LastName,
                                                                                          ue.EmailConfirmationToken);
                    if (emailSent)
                        this.AddInformationMessage("Confirmation email sent");
                    else
                        this.AddWarningMessage("Failed to send confirmation email.");

                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> ResendConfirmationEmail()
        {
            try
            {
                UserEntity ue = _usersRepo.GetUserByUsername(User.Identity.Name);

                ue.EmailConfirmed = false;
                ue.EmailConfirmationToken = Guid.NewGuid().ToString();
                ue.EmailConfirmationExpiration = DateTime.Now.AddHours(24);

                await _usersRepo.SaveAllAsync();

                bool emailSent = await _emailSender.SendRegisterConfirmationEmailAsync(ue.Email,
                                                                                      ue.FirstName,
                                                                                      ue.LastName,
                                                                                      ue.EmailConfirmationToken);

                if (emailSent)
                    this.AddInformationMessage("Confirmation email sent");
                else
                    this.AddWarningMessage("Failed to send confirmation email.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction("Details");
        }

        [HttpGet("[controller]/[action]/{token}", Name = "ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                UserEntity ue = _usersRepo.GetUserByConfirmationToken(token);
                if (User.Identity.IsAuthenticated && User.Identity.Name != ue.UserName)
                {
                    this.AddErrorMessage("You can't confirm a email that isn't yours when you are loged in!");
                    return RedirectToAction("Index", "Home");
                }

                bool emailConfirmed = false;

                if (ue != null)
                {
                    if (ue.EmailConfirmationExpiration > DateTime.Now)
                    {
                        ue.EmailConfirmed = true;
                        ue.EmailConfirmationToken = null;
                        await _usersRepo.SaveAllAsync();
                        emailConfirmed = ue.EmailConfirmed;
                    }
                }

                if (emailConfirmed)
                {
                    this.AddInformationMessage("Email confirmed!");
                    if (User.Identity.IsAuthenticated)
                        return RedirectToAction("Index", "Home");
                }
                else
                    this.AddErrorMessage("Invalid confirmation token");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return RedirectToAction("Login");
        }
    }
}
