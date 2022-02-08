using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ordering.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ordering.Functions
{
    public static class CreateOrderFunction
    {
        [FunctionName("CreateOrderFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            OrderModel orderModel)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>("CreateOrderFunction_ProcessPayment", orderModel));
            outputs.Add(await context.CallActivityAsync<string>("CreateOrderFunction_SendOrder", orderModel));

            return outputs;
        }

        [FunctionName("CreateOrderFunction_ProcessPayment")]
        public static string ProcessPayment([ActivityTrigger] OrderModel orderModel, ILogger log)
        {
            Order order = null;

            order.ProcessPayment();

            return $"Done";
        }

        [FunctionName("CreateOrderFunction_SendOrder")]
        public static string SendOrder([ActivityTrigger] OrderModel orderModel, ILogger log)
        {
            Order order = null;

            order.Ship();

            return $"Done";
        }

        [FunctionName("CreateOrderFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string requestBody = await req.Content.ReadAsStringAsync();
            var orderModel = JsonConvert.DeserializeObject<OrderModel>(requestBody);

            string instanceId = await starter.StartNewAsync("CreateOrderFunction", orderModel);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }

    public class OrderModel
    {
        public string CustomerId { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }

    public class OrderItemModel
    {
        public string ProductId { get; set; }
        public double Price { get; set; }
    }
}