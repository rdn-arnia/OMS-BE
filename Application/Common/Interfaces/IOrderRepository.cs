using OMS.Domain.Aggregates.OrderAggregate;
using System.Threading.Tasks;

namespace OMS.Application.Common.Interfaces
{
    public interface IOrderRepository
    {
        Task Add(Order order);
    }
}
