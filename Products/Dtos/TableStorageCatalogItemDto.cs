using Azure;
using Azure.Data.Tables;
using System;

namespace ProductCatalog.Dtos
{
    public class TableStorageCatalogItemDto : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string CatalogId { get; set; }
        public string ProductId { get; set; }
        public decimal Price { get; set; }
    }
}
