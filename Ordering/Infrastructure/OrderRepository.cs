using Azure;
using Azure.Data.Tables;
using Ordering.Domain;
using Ordering.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    class OrderRepository
    {
        private const string StorageTableOrders = "Orders";
        private const string StorageTableOrderItems = "OrderLines";

        private readonly TableClient orderTableClient;
        private readonly TableClient orderLineTableClient;

        public OrderRepository(string connectionString)
        {
            orderTableClient = new TableClient(connectionString, StorageTableOrders);
            orderLineTableClient = new TableClient(connectionString, StorageTableOrderItems);
        }

        public async Task<Order> GetByIdAsync(string orderId)
        {
            var orderDto = (await orderTableClient.GetEntityAsync<TableStorageOrderDto>("1", orderId)).Value;
            var orderLines = await orderLineTableClient.QueryAsync<TableStorageOrderLineDto>($"PartitionKey eq '{orderDto.RowKey}'").ToListAsync();

            var order = MapOrder(orderDto, orderLines);

            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            var orderDto = new TableStorageOrderDto
            {
                RowKey = order.OrderId,
                PartitionKey = "1",
                CustomerId = order.CustomerId,
                OrderStatus = order.OrderStatus.ToString()
            };
            await orderTableClient.UpdateEntityAsync(orderDto, ETag.All, TableUpdateMode.Replace);

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
                await orderLineTableClient.UpdateEntityAsync(orderLineDto, ETag.All, TableUpdateMode.Replace);
            }
        }

        public static Order MapOrder(TableStorageOrderDto tableStorageOrderDto, List<TableStorageOrderLineDto> tableStorageOrderLineDtos)
        {
            var order = (Order)Activator.CreateInstance(typeof(Order), true);
            order.GetType().GetProperty(nameof(Order.OrderId)).SetValue(order, tableStorageOrderDto.RowKey);
            order.GetType().GetProperty(nameof(Order.CustomerId)).SetValue(order, tableStorageOrderDto.CustomerId);
            order.GetType().GetProperty(nameof(Order.OrderStatus)).SetValue(order, Enum.Parse<OrderStatus>(tableStorageOrderDto.OrderStatus));

            var orderLines = new List<OrderLine>();
            foreach (var tableStorageOrderLineDto in tableStorageOrderLineDtos)
            {
                var orderLine = (OrderLine)Activator.CreateInstance(typeof(OrderLine), true);
                orderLine.GetType().GetProperty(nameof(OrderLine.OrderLineId)).SetValue(orderLine, tableStorageOrderLineDto.RowKey);
                orderLine.GetType().GetProperty(nameof(OrderLine.OrderId)).SetValue(orderLine, tableStorageOrderLineDto.OrderId);
                orderLine.GetType().GetProperty(nameof(OrderLine.Price)).SetValue(orderLine, tableStorageOrderLineDto.Price);
                orderLine.GetType().GetProperty(nameof(OrderLine.ProductId)).SetValue(orderLine, tableStorageOrderLineDto.ProductId);
                orderLine.GetType().GetProperty(nameof(OrderLine.Quantity)).SetValue(orderLine, tableStorageOrderLineDto.Quantity);

                orderLines.Add(orderLine);
            }

            order.GetType().GetField("_orderLines", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(order, orderLines);

            return order;
        }
    }
}
