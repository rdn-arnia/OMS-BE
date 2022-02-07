using BuildingBlocks;
using System;

namespace Products.Domain
{
    class Product : Entity
    {
        public Product(string title, string description)
        {
            Title = title;
            Description = description;

            ProductId = Guid.NewGuid();
        }

        public Guid ProductId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int CurrentStock { get; set; }
    }
}
