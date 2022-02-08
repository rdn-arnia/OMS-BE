using BuildingBlocks;
using System;

namespace Ordering.Domain
{
    internal class OrderLine : Entity
    {
        public OrderLine(string productId, int quantity, decimal price)
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;

            OrderLineId = Guid.NewGuid().ToString("n");
        }

        public string OrderLineId { get; set; }
        public string ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
    }
}
