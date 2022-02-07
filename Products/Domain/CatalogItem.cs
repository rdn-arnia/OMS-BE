using BuildingBlocks;

namespace Products.Domain
{
    internal class CatalogItem : Entity
    {
        public CatalogItem(Product product, decimal price)
        {
            Product = product;
            Price = price;
        }

        public Product Product { get; private set; }
        public decimal Price { get; private set; }
    }
}
