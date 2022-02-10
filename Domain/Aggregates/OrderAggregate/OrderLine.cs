using BuildingBlocks;
using System;

namespace OMS.Domain.Aggregates.OrderAggregate
{
    public class OrderLine : Entity
    {
        public OrderLine(string orderId, string productId, int quantity, double price)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;

            OrderLineId = Guid.NewGuid().ToString("n");
        }

        public string OrderLineId { get; set; }
        public string OrderId { get; }
        public string ProductId { get; private set; }
        public int Quantity { get; private set; }
        public double Price { get; private set; }
    }
}
