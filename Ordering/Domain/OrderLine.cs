using BuildingBlocks;

namespace Ordering.Domain
{
    internal class OrderLine : Entity
    {
        private OrderLine() { }

        public string OrderLineId { get; private set; }
        public string OrderId { get; private set; }
        public string ProductId { get; private set; }
        public int Quantity { get; private set; }
        public double Price { get; private set; }
    }
}
