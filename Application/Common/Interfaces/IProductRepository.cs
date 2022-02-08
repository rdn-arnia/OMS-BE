using OMS.Domain.Aggregates.ProductAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMS.Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Domain.Aggregates.ProductAggregate.Product?> GetByIdAsync(string productId);
    }
}
