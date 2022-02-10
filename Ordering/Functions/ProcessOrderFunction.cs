using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Ordering.Domain;
using Ordering.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Functions
{
    public static class ProcessOrderFunction
    {
        [FunctionName("ProcessOrderFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var orderModel = context.GetInput<OrderModel>();

            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>("ProcessOrderFunction_ProcessPayment", orderModel));
            outputs.Add(await context.CallActivityAsync<string>("ProcessOrderFunction_SendOrder", orderModel));

            return outputs;
        }

        [FunctionName("ProcessOrderFunction_ProcessPayment")]
        public static string ProcessPayment([ActivityTrigger] OrderModel orderModel, ILogger log)
        {
            Order order = null;

            order.ProcessPayment();

            return $"Done";
        }

        [FunctionName("ProcessOrderFunction_SendOrder")]
        public static string SendOrder([ActivityTrigger] OrderModel orderModel, ILogger log)
        {
            Order order = null;

            order.Ship();

            return $"Done";
        }

        [FunctionName("ProcessOrderFunction_QueueStart")]
        public static async Task QueueStart(
            [ServiceBusTrigger("order-created", Connection = "ServiceBusConnection")] OrderCreatedEvent order,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            //TODO: retrieve order from table storage
            var orderModel = new OrderModel
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderItems = new List<OrderItemModel>()
            };

            string instanceId = await starter.StartNewAsync("ProcessOrderFunction", orderModel);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }

    public class OrderModel
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }

    public class OrderItemModel
    {
        public string ProductId { get; set; }
        public double Price { get; set; }
    }
}