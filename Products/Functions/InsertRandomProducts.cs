using Azure.Data.Tables;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProductCatalog.Config;
using ProductCatalog.Dtos;
using System;
using System.Threading.Tasks;


namespace ProductCatalog.Functions
{
    static class InsertRandomProductsFunction
    {
        [FunctionName("InsertRandomProducts")]
        public static async Task<IActionResult> InsertRandomProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Running InsertRandomProducts");

            var count = int.Parse(req.GetQueryParameterDictionary()["count"]);

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), TableStorageConstants.Products);
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
    }
}
