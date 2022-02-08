using Azure.Data.Tables;
using OMS.Application.Common.Interfaces;
using OMS.Domain.Aggregates.CatalogAggregate;
using OMS.Infrastructure.Options;
using OMS.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Persistence
{
    class CatalogRepository : ICatalogRepository
    {
        private const string StorageTableCatalogs = "Catalogs";
        private const string StorageTableCatalogItems = "CatalogItems";

        private readonly TableClient productTableClient;
        private readonly TableClient catalogItemsTableClient;

        public CatalogRepository(AzureStorageOptions azureStorageOptions)
        {
            productTableClient = new TableClient(azureStorageOptions.ConnectionString, StorageTableCatalogs);
            catalogItemsTableClient = new TableClient(azureStorageOptions.ConnectionString, StorageTableCatalogItems);
        }

        public async Task<Catalog> GetCurrentCatalogWithoutCatalogItemsAsync()
        {
            var catalogResults = await productTableClient.QueryAsync<TableStorageCatalogDto>().ToListAsync();

            if (!catalogResults.Any())
            {
                return null;
            }

            var catalogResult = catalogResults.Last();

            var catalog = (Catalog)Activator.CreateInstance(typeof(Catalog), true);
            catalog.GetType().GetProperty(nameof(Catalog.CatalogId)).SetValue(catalog, catalogResult.RowKey);
            catalog.GetType().GetProperty(nameof(Catalog.CatalogName)).SetValue(catalog, catalogResult.CatalogName);

            return catalog;
        }

        public async Task<Catalog> GetCurrentCatalogAsync()
        {
            var catalog = await GetCurrentCatalogWithoutCatalogItemsAsync();

            var catalogItems = await GetCatalogItemsAsync(catalog.CatalogId);
            catalog.GetType().GetField("_catalogItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(catalog, catalogItems);

            return catalog;
        }

        private async Task<List<CatalogItem>> GetCatalogItemsAsync(string catalogId)
        {
            var catalogItemResults = await catalogItemsTableClient
                .QueryAsync<TableStorageCatalogItemDto>($"PartitionKey eq '{catalogId}'")
                .ToListAsync();

            var catalogItems = new List<CatalogItem>();
            foreach (var catalogItemResult in catalogItemResults)
            {
                var catalogItem = (CatalogItem)Activator.CreateInstance(typeof(CatalogItem), true);
                catalogItem.GetType().GetProperty(nameof(CatalogItem.CatalogItemId)).SetValue(catalogItem, catalogItemResult.RowKey);
                catalogItem.GetType().GetProperty(nameof(CatalogItem.ProductId)).SetValue(catalogItem, catalogItemResult.ProductId);
                catalogItem.GetType().GetProperty(nameof(CatalogItem.Price)).SetValue(catalogItem, catalogItemResult.Price);

                catalogItems.Add(catalogItem);
            }

            return catalogItems;
        }
    }
}
