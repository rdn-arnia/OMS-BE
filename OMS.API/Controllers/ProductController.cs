using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using OMS.Application.UseCases.Product.Queries.GetProductById;

namespace OMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(string productId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope("Order");

            var result = await mediator.Send(new GetProductByIdQuery { ProductId = productId });

            return Ok(result);
        }
    }
}
