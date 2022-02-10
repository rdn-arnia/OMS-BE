using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using OMS.Application.Common.EventBus;
using OMS.Infrastructure.Options;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OMS.Infrastructure.EventBus
{
    class AzureEventBus : IEventBus
    {
        private const string EventBusName = "event_bus_oms";
        private const string EventBusSubscriptionName = "Azure subscription 1";

        private readonly ServiceBusClient serviceBusClient;
        private readonly ServiceBusSender serviceBusSender;
        private readonly ServiceBusAdministrationClient subscriptionClient;
        private readonly ILogger<AzureEventBus> logger;
        private readonly IEventBusSubscriptionsManager subsManager;

        public AzureEventBus(AzureEventBusOptions azureEventBusOptions, ILogger<AzureEventBus> logger)
        {
            this.logger = logger;

            serviceBusClient = new ServiceBusClient(azureEventBusOptions.ConnectionString);
            serviceBusSender = serviceBusClient.CreateSender(EventBusName);
            subsManager = new InMemoryEventBusSubscriptionsManager();
            subscriptionClient = new ServiceBusAdministrationClient(azureEventBusOptions.ConnectionString);
        }

        public async Task Publish(Event @event)
        {
            var eventName = @event.GetType().Name;
            var jsonMessage = JsonSerializer.Serialize(@event, @event.GetType());
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new ServiceBusMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = new BinaryData(body),
                Subject = eventName,
            };

            await serviceBusSender.SendMessageAsync(message);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;

            var containsKey = subsManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                try
                {
                    subscriptionClient.CreateRuleAsync(EventBusName, EventBusSubscriptionName, new CreateRuleOptions
                    {
                        Filter = new CorrelationRuleFilter() { Subject = eventName },
                        Name = eventName
                    }).GetAwaiter().GetResult();
                }
                catch (ServiceBusException)
                {
                    logger.LogWarning("The messaging entity {eventName} already exists.", eventName);
                }
            }

            logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            subsManager.AddSubscription<T, TH>();
        }
    }
}
