using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;
namespace StackExchangeRedis.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absouluteExpireDate = null, TimeSpan? unUsedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absouluteExpireDate ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = unUsedExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
