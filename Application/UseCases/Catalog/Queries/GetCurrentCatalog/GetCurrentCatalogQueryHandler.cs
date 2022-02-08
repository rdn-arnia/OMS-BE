using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Catalog.Queries.GetCurrentCatalog
{
    class GetCurrentCatalogQueryHandler : IRequestHandler<GetCurrentCatalogQuery, CatalogDto>
    {
        private readonly ICatalogRepository catalogRepository;

        public GetCurrentCatalogQueryHandler(ICatalogRepository catalogRepository)
        {
            this.catalogRepository = catalogRepository;
        }

        public async Task<CatalogDto> Handle(GetCurrentCatalogQuery request, CancellationToken cancellationToken)
        {
            var catalog = await catalogRepository.GetCurrentCatalogWithoutCatalogItemsAsync();

            if (catalog == null)
            {
                throw new NotFoundException(nameof(Domain.Aggregates.CatalogAggregate.Catalog));
            }

            return new CatalogDto
            {
                CatalogId = catalog.CatalogId,
                CatalogName = catalog.CatalogName
            };
        }
    }
}
