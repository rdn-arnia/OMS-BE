using System.Threading.Tasks;

namespace OMS.Application.Common.EventBus
{
    public interface IEventBus
    {
        Task Publish(Event @event);

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
