using BuildingBlocks;
using System;
using System.Collections.Generic;

namespace Domain.Aggregates.CatalogAggregate
{
    public class Catalog : Entity, IAggregateRoot
    {
        private List<CatalogItem> _catalogItems;

        private Catalog()
        {
            _catalogItems = new List<CatalogItem>();
        }

        public Catalog(string catalogName)
            : this()
        {
            CatalogName = catalogName;

            CatalogId = Guid.NewGuid().ToString();
        }

        public string CatalogId { get; private set; }
        public string CatalogName { get; private set; }
        public IReadOnlyList<CatalogItem> CatalogItems => _catalogItems.AsReadOnly();
    }
}
