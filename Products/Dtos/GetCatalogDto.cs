using System.Collections.Generic;

namespace ProductCatalog.Dtos
{
    internal class GetCatalogDto
    {
        public string CatalogId { get; set; }
        public string CatalogName { get; set; }
        public List<CatalogItem> CatalogItems { get; set; }

        public class CatalogItem
        {
            public Product Product { get; set; }
            public double Price { get; set; }
        }

        public class Product
        {
            public string ProductId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int CurrentStock { get; set; }
        }
    }
}
