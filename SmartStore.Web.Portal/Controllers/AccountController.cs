using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;
using SmartStore.Web.Portal.Models;

namespace SmartStore.Web.Portal.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<UserEntity> _signInMgr;
        private UserManager<UserEntity> _userMgr;
        private IUsersRepository _usersRepo;
        private IMapper _mapper;

        public AccountController(SignInManager<UserEntity> signInMgr,
            UserManager<UserEntity> userMgr,
            IUsersRepository usersRepo,
            IMapper mapper)
        {
            _signInMgr = signInMgr;
            _userMgr = userMgr;
            _usersRepo = usersRepo;
            _mapper = mapper;
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
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Exception thrown while logging in: {ex}");
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
                UserEntity userEntity = _mapper.Map<UserEntity>(user);
                //validate password working
                IdentityResult result = await _userMgr.CreateAsync(userEntity, user.Password);
                if (result.Succeeded)
                {
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
    }
}
