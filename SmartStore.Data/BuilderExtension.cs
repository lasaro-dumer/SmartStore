using Microsoft.Extensions.DependencyInjection;
using SmartStore.Data.Initializers;
using SmartStore.Data.Repositories;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data
{
    public static class BuilderExtension
    {
        public static IServiceCollection AddSmartStoreData(this IServiceCollection services)
        {
            services.AddDbContext<SmartStoreDbContext>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IShoppingRepository, ShoppingRepository>();
            services.AddTransient<SmartStoreIdentityInitializer>();
            services.AddTransient<SmartStoreInitializer>();

            return services;
        }
    }
}
