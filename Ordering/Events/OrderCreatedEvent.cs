namespace Ordering.Events
{
    public class OrderCreatedEvent
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
    }
}
