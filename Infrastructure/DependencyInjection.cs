using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Common.Interfaces;
using OMS.Infrastructure.Options;
using OMS.Infrastructure.Persistence;

namespace OMS.Infrastructure
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
