using MediatR;

namespace OMS.Application.UseCases.Product.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public string ProductId { get; set; }
    }
}
