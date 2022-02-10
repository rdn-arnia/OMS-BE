using Azure.Data.Tables;
using OMS.Application.Common.Interfaces;
using OMS.Domain.Aggregates.OrderAggregate;
using OMS.Infrastructure.Options;
using OMS.Infrastructure.Persistence.Models;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Persistence
{
    class OrderRepository : IOrderRepository
    {
        private const string StorageTableOrders = "Orders";
        private const string StorageTableOrderItems = "OrderLines";

        private readonly TableClient orderTableClient;
        private readonly TableClient orderLineTableClient;

        public OrderRepository(AzureStorageOptions azureStorageOptions)
        {
            orderTableClient = new TableClient(azureStorageOptions.ConnectionString, StorageTableOrders);
            orderLineTableClient = new TableClient(azureStorageOptions.ConnectionString, StorageTableOrderItems);
        }

        public async Task Add(Order order)
        {
            await orderTableClient.CreateIfNotExistsAsync();
            await orderLineTableClient.CreateIfNotExistsAsync();

            var orderDto = new TableStorageOrderDto
            {
                RowKey = order.OrderId,
                PartitionKey = "1",
                CustomerId = order.CustomerId,
                OrderStatus = order.OrderStatus.ToString()
            };
            await orderTableClient.AddEntityAsync(orderDto);

            foreach (var orderLine in order.OrderLines)
            {
                var orderLineDto = new TableStorageOrderLineDto
                {
                    RowKey = orderLine.OrderLineId,
                    PartitionKey = orderLine.OrderId,
                    OrderId = orderLine.OrderId,
                    Price = orderLine.Price,
                    ProductId = orderLine.ProductId,
                    Quantity = orderLine.Quantity,
                };
                await orderLineTableClient.AddEntityAsync(orderLineDto);
            }
        }
    }
}
