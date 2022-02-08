using MediatR;
using OMS.Application.Common.Exceptions;
using OMS.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OMS.Application.UseCases.Catalog.Queries.GetCurrentCatalog
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
