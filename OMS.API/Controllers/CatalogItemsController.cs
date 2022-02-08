using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMS.Application.UseCases.CatalogItem.Queries.GetCatalogItems;

namespace OMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogItemsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CatalogItemsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("Catalog/{catalogId}")]
        public async Task<IActionResult> GetByCatalogId(string catalogId)
        {
            var catalog = await mediator.Send(new GetCatalogItemsQuery { CatalogId = catalogId });

            return Ok(catalog);
        }
    }
}
