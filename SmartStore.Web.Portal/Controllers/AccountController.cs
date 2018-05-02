using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;
using SmartStore.Web.Portal.Utility;
using SmartStore.Web.Portal.Models;
using System.Threading.Tasks;

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

        public AccountController(SignInManager<UserEntity> signInMgr,
            UserManager<UserEntity> userMgr,
            IUsersRepository usersRepo,
            IMapper mapper,
            ILogger<AccountController> logger,
            EmailSender emailSender)
        {
            _signInMgr = signInMgr;
            _userMgr = userMgr;
            _usersRepo = usersRepo;
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
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
        public async System.Threading.Tasks.Task<IActionResult> Login([FromForm]CredentialModel credential)
        {
            try
            {
                var result = await _signInMgr.PasswordSignInAsync(credential.UserName, credential.Password, false, false);

                if (result.Succeeded)
                {
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
                    await _emailSender.SendRegisterConfirmationEmailAsync(ue.Email,
                                                                          ue.FirstName,
                                                                          ue.LastName,
                                                                          ue.EmailConfirmationToken);
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

        [HttpGet("[controller]/[action]/{token}", Name = "ConfirmEmail")]
        public IActionResult ConfirmEmail(string token)
        {
            UserEntity ue = _usersRepo.GetUserByConfirmationToken(token);
            bool emailConfirmed = false;
            if (ue != null)
            {
                if (ue.EmailConfirmationExpiration > DateTime.Now)
                {
                    ue.EmailConfirmed = true;
                    ue.EmailConfirmationToken = null;
                    _usersRepo.SaveAllAsync();
                    emailConfirmed = ue.EmailConfirmed;
                }
            }

            if (emailConfirmed)
                this.AddInformationMessage("Email confirmed!");
            else
                this.AddErrorMessage("Invalid confirmation token");

            return RedirectToAction("Login");
        }
    }
}
