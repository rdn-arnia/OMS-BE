using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCatalog.Config;
using ProductCatalog.Dtos;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Functions
{
    internal class InsertProductsFunction
    {
        [FunctionName("InsertProducts")]
        public static async Task<IActionResult> InsertProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Running InsertProducts");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var products = JsonConvert.DeserializeObject<InsertProductDto[]>(requestBody);

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), TableStorageConstants.Products);
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
    }
}
