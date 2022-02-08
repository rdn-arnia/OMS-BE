using BuildingBlocks;
using System;

namespace Domain.Aggregates.ProductAggregate
{
    public class Product : Entity, IAggregateRoot
    {
        private Product()
        {

        }

        public Product(string title, string description, int currentStock)
            : this()
        {
            Title = title;
            Description = description;

            ProductId = Guid.NewGuid().ToString();
            CurrentStock = currentStock;
        }

        public string ProductId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int CurrentStock { get; private set; }
    }
}
