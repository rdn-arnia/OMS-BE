using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProductCatalog.Config;
using ProductCatalog.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Products
{
    public static class GetCatalogFunctions
    {
        [FunctionName("GetCatalog")]
        public static async Task<IActionResult> GetCatalog(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.AzureWebJobsStorage);

            var catalogTableClient = new TableClient(connectionString, TableStorageConstants.Catalogs);
            var catalogItemsTableClient = new TableClient(connectionString, TableStorageConstants.CatalogItems);
            var productTableClient = new TableClient(connectionString, TableStorageConstants.Products);

            var catalog = await catalogTableClient.QueryAsync<TableStorageCatalogDto>().LastOrDefaultAsync();
            var catalogItems = await catalogItemsTableClient.QueryAsync<TableStorageCatalogItemDto>($"PartitionKey eq '{catalog.RowKey}'").ToListAsync();
            var products = await productTableClient.QueryAsync<TableStorageProductDto>().ToListAsync();

            var model = new GetCatalogDto
            {
                CatalogId = catalog.RowKey,
                CatalogName = catalog.CatalogName,
                CatalogItems = new System.Collections.Generic.List<GetCatalogDto.CatalogItem>()
            };

            foreach (var catalogItem in catalogItems)
            {
                var product = products.First(p => p.RowKey == catalogItem.ProductId);
                model.CatalogItems.Add(new GetCatalogDto.CatalogItem
                {
                    Price = catalogItem.Price,
                    Product = new GetCatalogDto.Product
                    {
                        ProductId = product.RowKey,
                        CurrentStock = product.CurrentStock,
                        Description = product.Description,
                        Title = product.Title
                    }
                });
            }

            return new OkObjectResult(model);
        }
    }
}
