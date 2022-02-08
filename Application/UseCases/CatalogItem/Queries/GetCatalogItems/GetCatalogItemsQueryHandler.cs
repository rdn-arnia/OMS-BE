using MediatR;
using OMS.Application.Common.Exceptions;
using OMS.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMS.Application.UseCases.CatalogItem.Queries.GetCatalogItems
{
    class GetCatalogItemsQueryHandler : IRequestHandler<GetCatalogItemsQuery, List<CatalogItemDto>>
    {
        private readonly ICatalogRepository catalogRepository;

        public GetCatalogItemsQueryHandler(ICatalogRepository catalogRepository)
        {
            this.catalogRepository = catalogRepository;
        }

        public async Task<List<CatalogItemDto>> Handle(GetCatalogItemsQuery request, CancellationToken cancellationToken)
        {
            var catalog = await catalogRepository.GetCurrentCatalogAsync();

            if (catalog == null)
            {
                throw new NotFoundException(nameof(Domain.Aggregates.CatalogAggregate.Catalog));
            }

            return catalog.CatalogItems.Select(ci => new CatalogItemDto
            {
                CatalogItemId = ci.CatalogItemId,
                Price = ci.Price,
                ProductId = ci.ProductId
            }).ToList();
        }
    }
}
