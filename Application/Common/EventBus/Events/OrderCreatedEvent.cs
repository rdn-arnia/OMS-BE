namespace OMS.Application.Common.EventBus.Events
{
    public class OrderCreatedEvent : Event
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
    }
}
