using BuildingBlocks;

namespace Ordering.Domain
{
    internal class OrderLine : Entity
    {
        public OrderLine(int productId, int quantity, decimal price)
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public int OrderLineId { get; set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
    }
}
