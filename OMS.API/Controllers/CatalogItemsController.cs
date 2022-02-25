using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using OMS.Application.UseCases.CatalogItem.Queries.GetCatalogItems;

namespace OMS.API.Controllers
{
    [Authorize]
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
            HttpContext.VerifyUserHasAnyAcceptedScope("Order");

            var catalog = await mediator.Send(new GetCatalogItemsQuery { CatalogId = catalogId });

            return Ok(catalog);
        }
    }
}
