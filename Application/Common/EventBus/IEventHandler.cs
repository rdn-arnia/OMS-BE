using System.Threading.Tasks;

namespace OMS.Application.Common.EventBus
{
    public interface IEventHandler<T>
        where T : Event
    {
        Task Handle<T>(T @event);
    }
}
