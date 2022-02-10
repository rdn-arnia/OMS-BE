using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Ordering.Events;
using Ordering.Infrastructure;
using System;
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
            var orderId = context.GetInput<string>();

            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>("ProcessOrderFunction_ProcessPayment", orderId));
            outputs.Add(await context.CallActivityAsync<string>("ProcessOrderFunction_SendOrder", orderId));

            return outputs;
        }

        [FunctionName("ProcessOrderFunction_ProcessPayment")]
        public static async Task<string> ProcessPaymentAsync([ActivityTrigger] string orderId, ILogger log)
        {
            var orderRepository = CreateOrderRepository();

            var order = await orderRepository.GetByIdAsync(orderId);

            log.LogInformation("Processing payment");
            order.ProcessPayment();
            log.LogInformation("Payment completed");

            await orderRepository.UpdateAsync(order);

            return $"Done";
        }

        [FunctionName("ProcessOrderFunction_SendOrder")]
        public static async Task<string> SendOrderAsync([ActivityTrigger] string orderId, ILogger log)
        {
            var orderRepository = CreateOrderRepository();

            var order = await orderRepository.GetByIdAsync(orderId);

            log.LogInformation("Shipping order");
            order.Ship();
            log.LogInformation("Order shipped");

            await orderRepository.UpdateAsync(order);

            return $"Done";
        }

        [FunctionName("ProcessOrderFunction_QueueStart")]
        public static async Task QueueStart(
            [ServiceBusTrigger("order-created", Connection = "ServiceBusConnection")] OrderCreatedEvent orderEvent,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            OrderRepository orderRepository = CreateOrderRepository();

            var order = await orderRepository.GetByIdAsync(orderEvent.OrderId);

            string instanceId = await starter.StartNewAsync("ProcessOrderFunction", null, order.OrderId);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }

        private static OrderRepository CreateOrderRepository()
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            var orderRepository = new OrderRepository(connectionString);
            return orderRepository;
        }
    }
}