using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.CatalogItem.Queries.GetCatalogItems
{
    public class GetCatalogItemsQuery : IRequest<List<CatalogItemDto>>
    {
        public string CatalogId { get; set; }
    }
}
