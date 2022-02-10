using Azure;
using Azure.Data.Tables;
using System;

namespace Ordering.Infrastructure.Models
{
    class TableStorageOrderLineDto : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
