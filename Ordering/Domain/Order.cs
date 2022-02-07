using BuildingBlocks;
using System;
using System.Collections.Generic;

namespace Ordering.Domain
{
    internal class Order : Entity, IAggregateRoot
    {
        private List<OrderLine> _orderLines;

        public Order(int customerId, List<OrderLine> orderLines)
        {
            _orderLines = orderLines;
            CustomerId = customerId;

            OrderStatus = OrderStatus.ReadyToBeFulfilled;
        }

        public int OrderId { get; private set; }
        public int CustomerId { get; private set; }
        public IReadOnlyList<OrderLine> OrderLines { get => _orderLines; }
        public OrderStatus OrderStatus { get; private set; }

        public void ProcessPayment()
        {
            if (OrderStatus != OrderStatus.ReadyToBeFulfilled)
            {
                throw new Exception("Order status has to be ReadyToBeFulfilled");
            }

            OrderStatus = OrderStatus.PaymentProceesed;
        }

        public void Fulfil()
        {
            if (OrderStatus != OrderStatus.PaymentProceesed)
            {
                throw new Exception("Order status has to be PaymentProceesed");
            }

            OrderStatus = OrderStatus.Fulfilled;
        }
    }
}
