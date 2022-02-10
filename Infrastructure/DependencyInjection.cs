using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Common.EventBus;
using OMS.Application.Common.Interfaces;
using OMS.Infrastructure.EventBus;
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
            services.AddTransient(provider =>
                new AzureEventBusOptions { ConnectionString = configuration.GetConnectionString("AzureServiceBus") });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICatalogRepository, CatalogRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IEventBus, AzureEventBus>();

            return services;
        }
    }
}
