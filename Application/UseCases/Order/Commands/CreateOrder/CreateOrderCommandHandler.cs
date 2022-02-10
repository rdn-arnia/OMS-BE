using MediatR;
using OMS.Application.Common.EventBus;
using OMS.Application.Common.EventBus.Events;
using OMS.Application.Common.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMS.Application.UseCases.Order.Commands.CreateOrder
{
    class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly ICatalogRepository catalogRepository;
        private readonly IEventBus eventBus;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, 
            IProductRepository productRepository, 
            ICatalogRepository catalogRepository, 
            IEventBus eventBus)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.catalogRepository = catalogRepository;
            this.eventBus = eventBus;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Domain.Aggregates.OrderAggregate.Order(request.CustomerId);
            var productStock = (await productRepository.GetAllProducts()).ToDictionary(p => p.ProductId, p => p.CurrentStock);
            var catalogProductPrices = (await catalogRepository.GetCurrentCatalogAsync()).CatalogItems.ToDictionary(ci => ci.ProductId, ci => ci.Price);

            foreach (var orderArticle in request.OrderArticles)
            {
                var price = catalogProductPrices[orderArticle.ProductId];
                if (orderArticle.Quantity > productStock[orderArticle.ProductId])
                {
                    throw new Exception($"Not enough stock for product id {orderArticle.ProductId}");
                }

                order.AddOrderLine(orderArticle.ProductId, orderArticle.Quantity, price);
            }

            await orderRepository.Add(order);
            await eventBus.Publish(new OrderCreatedEvent
            {
                CustomerId = request.CustomerId,
                OrderId = order.OrderId
            });

            return Unit.Value;
        }
    }
}
