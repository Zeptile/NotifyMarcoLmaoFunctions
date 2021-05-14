using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Zeptile.NotifyMarcoLmao.Shared.Helpers
{
    public static class CosmosDbExtensions
    {
        /// <summary>
        /// Convert a feed iterator to IAsyncEnumerable
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="setIterator"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<TModel> ToAsyncEnumerable<TModel>(this FeedIterator<TModel> setIterator)
        {
            while (setIterator.HasMoreResults)
                foreach (var item in await setIterator.ReadNextAsync())
                {
                    yield return item;
                }
        }
    }
    
    public static class AsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items,
            CancellationToken cancellationToken = default)
        {
            var results = new List<T>();
            await foreach (var item in items.WithCancellation(cancellationToken)
                .ConfigureAwait(false))
                results.Add(item);
            return results;
        }
    }
}