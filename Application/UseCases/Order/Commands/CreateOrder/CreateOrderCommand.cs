using MediatR;
using System.Collections.Generic;

namespace OMS.Application.UseCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest
    {
        public string CustomerId { get; set; }
        public List<OrderArticle> OrderArticles { get; set; }

        public class OrderArticle
        {
            public string ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
