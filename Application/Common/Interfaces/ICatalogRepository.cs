using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICatalogRepository
    {
        Task<Domain.Aggregates.CatalogAggregate.Catalog> GetCurrentCatalogAsync();
        Task<Domain.Aggregates.CatalogAggregate.Catalog> GetCurrentCatalogWithoutCatalogItemsAsync();
    }
}
