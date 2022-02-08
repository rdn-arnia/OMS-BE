using Azure.Data.Tables;
using OMS.Application.Common.Interfaces;
using OMS.Domain.Aggregates.OrderAggregate;
using OMS.Infrastructure.Options;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Persistence
{
    class OrderRepository : IOrderRepository
    {
        private const string StorageTableOrders = "Orders";

        private readonly TableClient orderTableClient;

        public OrderRepository(AzureStorageOptions azureStorageOptions)
        {
            orderTableClient = new TableClient(azureStorageOptions.ConnectionString, StorageTableOrders);
        }

        public async Task Add(Order order)
        {
            await orderTableClient.CreateIfNotExistsAsync();

            //TODO: create order and order items
        }
    }
}
