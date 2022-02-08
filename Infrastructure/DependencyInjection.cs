using Application.Common.Interfaces;
using Infrastructure.Options;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(provider => 
                new AzureStorageOptions { ConnectionString = configuration.GetConnectionString("AzureWebJobsStorage") });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICatalogRepository, CatalogRepository>();

            return services;
        }
    }
}
