using MediatR;
using Microsoft.AspNetCore.Mvc;
using OMS.Application.UseCases.Order.Commands.CreateOrder;

namespace OMS.API.Controllers
{
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
            await mediator.Send(createOrderCommand);

            return Ok();
        }
    }
}
