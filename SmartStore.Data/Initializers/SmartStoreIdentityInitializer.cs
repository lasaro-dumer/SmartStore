using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SmartStore.Data.Entities;

namespace SmartStore.Data.Initializers
{
    public class SmartStoreIdentityInitializer
    {

        private RoleManager<IdentityRole> _roleMgr;
        private IConfiguration _config;
        private UserManager<UserEntity> _userMgr;

        public SmartStoreIdentityInitializer(
            UserManager<UserEntity> userMgr,
            RoleManager<IdentityRole> roleMgr,
            IConfiguration config)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            _config = config;
        }

        public async Task Seed()
        {
            var user = await _userMgr.FindByNameAsync("storesysadmin");

            // Add User
            if (user == null)
            {
                if (!(await _roleMgr.RoleExistsAsync("Admin")))
                {
                    var role = new IdentityRole("Admin");
                    var adminClaim = new IdentityRoleClaim<string>() { ClaimType = "IsAdmin", ClaimValue = "True" };
                    await _roleMgr.CreateAsync(role);
                    await _roleMgr.AddClaimAsync(role, adminClaim.ToClaim());
                }

                user = new UserEntity()
                {
                    UserName = "storesysadmin",
                    FirstName = "Store",
                    LastName = "SysAdmin",
                    Email = _config["sysadmin_email"]
                };

                var userResult = await _userMgr.CreateAsync(user, _config["sysadmin_password"]);
                var roleResult = await _userMgr.AddToRoleAsync(user, "Admin");
                var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
        }
    }
}
