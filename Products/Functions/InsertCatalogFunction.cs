using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProductCatalog.Config;
using ProductCatalog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Functions
{
    static class InsertCatalogFunction
    {
        [FunctionName("InsertCatalog")]
        public static async Task<IActionResult> InsertCatalog(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Running InsertCatalog");

            var productTableClient = new TableClient(Environment.GetEnvironmentVariable(EnvironmentVariableConstants.AzureWebJobsStorage), TableStorageConstants.Products);
            var results = productTableClient.QueryAsync<TableStorageProductDto>();

            var productEntities = new List<TableStorageProductDto>();
            await foreach (var entity in results)
            {
                productEntities.Add(entity);
            }

            var random = new Random();

            var catalog = new TableStorageCatalogDto()
            {
                CatalogName = "Summer catalog",
                RowKey = Guid.NewGuid().ToString("n"),
                PartitionKey = "1"
            };

            var catalogTableClient = new TableClient(Environment.GetEnvironmentVariable(EnvironmentVariableConstants.AzureWebJobsStorage), TableStorageConstants.Catalogs);
            await catalogTableClient.CreateIfNotExistsAsync();
            var response = await catalogTableClient.AddEntityAsync(catalog);

            var catalogItems = productEntities.Select(p => new TableStorageCatalogItemDto()
            {
                PartitionKey = catalog.RowKey,
                RowKey = Guid.NewGuid().ToString("n"),
                ProductId = p.RowKey,
                Price = random.Next(1, 100)
            }).ToList();

            var catalogItemsTableClient = new TableClient(Environment.GetEnvironmentVariable(EnvironmentVariableConstants.AzureWebJobsStorage), TableStorageConstants.CatalogItems);
            await catalogItemsTableClient.CreateIfNotExistsAsync();
            foreach (var catalogItem in catalogItems)
            {
                await catalogItemsTableClient.AddEntityAsync(catalogItem);
            }

            return new OkObjectResult(catalog);
        }
    }
}
