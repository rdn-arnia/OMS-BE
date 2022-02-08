using MediatR;
using OMS.Application.Common.Exceptions;
using OMS.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OMS.Application.UseCases.Product.Queries.GetProductById
{
    class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            return new ProductDto
            {
                ProductId = product.ProductId,
                Title = product.Title,
                CurrentStock = product.CurrentStock,
                Description = product.Description
            };
        }
    }
}
