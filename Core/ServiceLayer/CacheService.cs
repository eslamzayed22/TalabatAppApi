using DomainLayer.Contracts;
using ServiceAbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CacheService (ICacheRepository _cacheRepository) : ICacheService
    {
        public async Task<string?> GetAsync(string key)
        {
            return await _cacheRepository.GetAsync(key);
        }

        public async Task SetAsync(string key, object value, TimeSpan timeToLive)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var valueString = JsonSerializer.Serialize(value, options);
            await _cacheRepository.SetAsync(key, valueString, timeToLive);
        }
    }
}
