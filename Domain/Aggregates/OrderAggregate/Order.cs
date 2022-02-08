using BuildingBlocks;
using System;
using System.Collections.Generic;

namespace OMS.Domain.Aggregates.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private List<OrderLine> _orderLines;

        private Order()
        {
            _orderLines = new List<OrderLine>();
        }

        public Order(string customerId)
            : this()
        {
            CustomerId = customerId;

            OrderId = Guid.NewGuid().ToString("n");
            OrderStatus = OrderStatus.ReadyToBeFulfilled;
        }

        public string OrderId { get; private set; }
        public string CustomerId { get; private set; }
        public IReadOnlyList<OrderLine> OrderLines { get => _orderLines; }
        public OrderStatus OrderStatus { get; private set; }

        public void AddOrderLine(string productId, int quantity, double price)
        {
            _orderLines.Add(new OrderLine(productId, quantity, price));
        }

        public void ProcessPayment()
        {
            if (OrderStatus != OrderStatus.ReadyToBeFulfilled)
            {
                throw new Exception("Order status has to be ReadyToBeFulfilled");
            }

            OrderStatus = OrderStatus.PaymentProceesed;
        }

        public void Ship()
        {
            if (OrderStatus != OrderStatus.PaymentProceesed)
            {
                throw new Exception("Order status has to be PaymentProceesed");
            }

            OrderStatus = OrderStatus.Fulfilled;
        }
    }
}
