using Application.UseCases.Catalog.Queries.GetCurrentCatalog;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CatalogsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("Current")]
        public async Task<IActionResult> GetCurrentCatalog()
        {
            var catalog = await mediator.Send(new GetCurrentCatalogQuery());

            return Ok(catalog);
        }
    }
}
