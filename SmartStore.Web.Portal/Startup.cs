using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartStore.Data;
using SmartStore.Data.Entities;
using SmartStore.Data.Initializers;
using SmartStore.Web.Portal.Models;
using SmartStore.Web.Portal.Utility;

namespace SmartStore.Web.Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSmartStoreData();
            services.AddAutoMapper();

            services.AddSingleton<EmailSender>();

            services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<SmartStoreDbContext>();

            services.AddMvc();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            SmartStoreInitializer smartStoreInitializer,
            SmartStoreIdentityInitializer identityInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<SerilogMiddleware>();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            app.Use((httpContext, nextMiddleware) =>
            {
                var sessionCart = httpContext.Session.Get<CartModel>(SessionExtensions.SessionCart);
                if(sessionCart == null)
                    httpContext.Session.Set(SessionExtensions.SessionCart, new CartModel());

                return nextMiddleware();
            });

            app.UseMvcWithDefaultRoute();

            smartStoreInitializer.Seed().Wait();
            identityInitializer.Seed().Wait();
        }
    }
}
