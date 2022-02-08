using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;

namespace OMS.Infrastructure.Persistence.Models
{
    class TableStorageCatalogDto : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string CatalogName { get; set; }
    }
}
