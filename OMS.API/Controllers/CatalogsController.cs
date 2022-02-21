using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using OMS.Application.UseCases.Catalog.Queries.GetCurrentCatalog;

namespace OMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogsController : ControllerBase
    {
        private readonly IMediator mediator;

        static readonly string[] ScopeRequiredByApi = new string[] { "Order" };

        public CatalogsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("Current")]
        public async Task<IActionResult> GetCurrentCatalog()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(ScopeRequiredByApi);

            var catalog = await mediator.Send(new GetCurrentCatalogQuery());

            return Ok(catalog);
        }
    }
}
