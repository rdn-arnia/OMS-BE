using Azure;
using Azure.Data.Tables;
using System;

namespace OMS.Infrastructure.Persistence.Models
{
    class TableStorageOrderDto : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string CustomerId { get; set; }
        public string OrderStatus { get; set; }
    }
}
