using Microsoft.Extensions.DependencyInjection;
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
            services.AddTransient<SmartStoreIdentityInitializer>();

            return services;
        }
    }
}
