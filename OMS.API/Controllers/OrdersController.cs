using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using OMS.Application.UseCases.Order.Commands.CreateOrder;

namespace OMS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CreateOrderCommand createOrderCommand)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope("Order");

            await mediator.Send(createOrderCommand);

            return Ok();
        }
    }
}
