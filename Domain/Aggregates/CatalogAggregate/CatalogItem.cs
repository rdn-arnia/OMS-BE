using BuildingBlocks;
using System;

namespace OMS.Domain.Aggregates.CatalogAggregate
{
    public class CatalogItem : Entity
    {
        private CatalogItem()
        {

        }

        public CatalogItem(string productId, double price)
            : this()
        {
            ProductId = productId;
            Price = price;

            CatalogItemId = Guid.NewGuid().ToString();
        }

        public string CatalogItemId { get; private set; }
        public string ProductId { get; private set; }
        public double Price { get; private set; }
    }
}
