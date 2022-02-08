using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<Domain.Aggregates.ProductAggregate.Product?> GetByIdAsync(string productId);
    }
}
