using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCatalog.Dtos;
using Azure.Data.Tables;
using System.Linq;
using Bogus;
using System.Collections.Generic;

namespace Products
{
    public static class ProductCatalogFunctions
    {
        private const string ProductsTableName = "Products";

        [FunctionName("Function1")]
        public static async Task<IActionResult> Function1(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("InsertProducts")]
        public static async Task<IActionResult> InsertProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null )] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Running InsertProducts");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var products = JsonConvert.DeserializeObject<InsertProductDto[]>(requestBody);

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), ProductsTableName);
            await tableClient.CreateIfNotExistsAsync();

            var entities = products.Select(p => new TableStorageProductDto
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString("n"),
                CurrentStock = p.CurrentStock,
                Description = p.Description,
                Title = p.Title
            });
            
            foreach (var entity in entities)
            {
                await tableClient.AddEntityAsync(entity);
            }

            return new OkObjectResult(entities);
        }

        [FunctionName("InsertRandomProducts")]
        public static async Task<IActionResult> InsertRandomProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Running InsertRandomProducts");

            var count = int.Parse(req.GetQueryParameterDictionary()["count"]);

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), ProductsTableName);
            await tableClient.CreateIfNotExistsAsync();

            var productEntities = new Faker<TableStorageProductDto>()
                .RuleFor(o => o.PartitionKey, "1")
                .RuleFor(o => o.RowKey, f => f.Commerce.Ean13())
                .RuleFor(o => o.Title, f => f.Commerce.ProductName())
                .RuleFor(o => o.Description, f => f.Commerce.ProductDescription())
                .RuleFor(o => o.CurrentStock, f => f.Random.Int(1, 100))
                .Generate(count);

            foreach (var productEntity in productEntities)
            {
                await tableClient.AddEntityAsync(productEntity);
            }

            return new OkObjectResult(productEntities);
        }

        [FunctionName("InsertCatalog")]
        public static async Task<IActionResult> InsertCatalog(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Running InsertCatalog");

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), ProductsTableName);
            var results = tableClient.QueryAsync<TableStorageProductDto>();

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

            var catalogTableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "Catalogs");
            await catalogTableClient.CreateIfNotExistsAsync();
            var response = await catalogTableClient.AddEntityAsync(catalog);

            var catalogItems = productEntities.Select(p => new TableStorageCatalogItemDto()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString("n"),
                ProductId = p.RowKey,
                Price = random.Next(1, 100)
            }).ToList();

            var catalogItemsTableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "CatalogItems");
            await catalogItemsTableClient.CreateIfNotExistsAsync();
            foreach (var catalogItem in catalogItems)
            {
                await catalogItemsTableClient.AddEntityAsync(catalogItem);
            }

            return new OkObjectResult(catalog);
        }
    }
}
