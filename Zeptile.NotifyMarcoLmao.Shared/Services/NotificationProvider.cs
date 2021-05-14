using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using Zeptile.NotifyMarcoLmao.Shared.Helpers;
using Zeptile.NotifyMarcoLmao.Shared.Interfaces;
using Zeptile.NotifyMarcoLmao.Shared.Models;

namespace Zeptile.NotifyMarcoLmao.Shared.Services
{
    public class NotificationProvider : INotificationProvider
    {
        private readonly IConfiguration _config;
        private readonly Container _container;

        public NotificationProvider(IConfiguration config)
        {
            _config = config;
            
            var client = new CosmosClient(_config["CosmosDbEndpoint"], _config["CosmosDbAuthKey"], new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });
            
            var db = client.GetDatabase("NotifyMarcoLmao");
            _container = db.GetContainer("notificationitems");
        }

        public async Task<List<NotificationItem>> GetAllItemsAsync()
        {
            return await QueryCosmosMultipleWhereAsync<NotificationItem>(x => true);
        }
        
        public async Task<List<NotificationItem>> GetItemWhereDue()
        {
            return await QueryCosmosMultipleWhereAsync<NotificationItem>
                (x => x.DueDate < DateTime.Now && x.Status == NotificationStatus.Pending);
        }

        public async Task InsertItem(NotificationItem item)
        {
            // Using UUID to identify rows
            item.Id = Guid.NewGuid().ToString();
            await _container.CreateItemAsync(item);
        }
        
        public async Task UpdateItem(NotificationItem item)
        {
            await _container.ReplaceItemAsync(item, item.Id);
        }

        private async Task<List<T>> QueryCosmosMultipleWhereAsync<T>(Expression<Func<T, bool>> whereFunc)
        {
            using var feedIterator = _container.GetItemLinqQueryable<T>()
                    .Where(whereFunc)
                    .ToFeedIterator();
            
            return await feedIterator.ToAsyncEnumerable().ToListAsync();
        }

    }
}