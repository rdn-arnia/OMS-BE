using BuildingBlocks;
using System.Collections.Generic;

namespace Products.Domain
{
    internal class Catalog : Entity, IAggregateRoot
    {
        private List<CatalogItem> _catalogItems;

        public Catalog(string catalogName)
        {
            CatalogName = catalogName;

            _catalogItems = new List<CatalogItem>();
        }

        public string CatalogId { get; private set; }
        public string CatalogName { get; private set; }
        public IReadOnlyList<CatalogItem> CatalogItems => _catalogItems.AsReadOnly();
    }
}
