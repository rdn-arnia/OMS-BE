using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using OMS.Application.Common.Interfaces;
using OMS.Domain.Aggregates.ProductAggregate;
using OMS.Infrastructure.Options;
using OMS.Infrastructure.Persistence.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Persistence
{
    class ProductRepository : IProductRepository
    {
        private const string StorageTableProducts = "Products";

        private readonly TableClient productTableClient;

        public ProductRepository(AzureStorageOptions azureStorageOptions)
        {
            productTableClient = new TableClient(azureStorageOptions.ConnectionString, StorageTableProducts);
        }

        public async Task<Product?> GetByIdAsync(string productId)
        {
            var productResults = await productTableClient.QueryAsync<TableStorageProductDto>($"RowKey eq '{productId}'").ToListAsync();

            if (!productResults.Any())
            {
                return null;
            }

            var productResult = productResults.First();

            var product = (Product)Activator.CreateInstance(typeof(Product), true);
            product.GetType().GetProperty(nameof(Product.ProductId)).SetValue(product, productResult.RowKey);
            product.GetType().GetProperty(nameof(Product.Title)).SetValue(product, productResult.Title);
            product.GetType().GetProperty(nameof(Product.Description)).SetValue(product, productResult.Description);
            product.GetType().GetProperty(nameof(Product.CurrentStock)).SetValue(product, productResult.CurrentStock);

            return product;
        }
    }
}
