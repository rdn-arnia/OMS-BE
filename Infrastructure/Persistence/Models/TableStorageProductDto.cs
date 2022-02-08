using Azure;
using Azure.Data.Tables;
using System;

namespace Infrastructure.Persistence.Models
{
    class TableStorageProductDto : ITableEntity
    {
        public string PartitionKey { get; set; } = "1";
        public string RowKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CurrentStock { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
