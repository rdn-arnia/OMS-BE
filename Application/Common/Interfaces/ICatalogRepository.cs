using OMS.Domain.Aggregates.CatalogAggregate;
using System.Threading.Tasks;

namespace OMS.Application.Common.Interfaces
{
    public interface ICatalogRepository
    {
        Task<Catalog> GetCurrentCatalogAsync();
        Task<Catalog> GetCurrentCatalogWithoutCatalogItemsAsync();
    }
}
