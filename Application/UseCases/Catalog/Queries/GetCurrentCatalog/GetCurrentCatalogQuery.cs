using MediatR;

namespace Application.UseCases.Catalog.Queries.GetCurrentCatalog
{
    public class GetCurrentCatalogQuery : IRequest<CatalogDto>
    {
    }
}
