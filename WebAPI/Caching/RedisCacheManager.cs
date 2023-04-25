using StackExchange.Redis;
using System.Text.Json;

namespace WebAPI.Caching
{
    public class RedisCacheManager
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCacheManager(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _database = _redis.GetDatabase();
        }

        public ConnectionMultiplexer Connection => _redis;

        public T Get<T>(string key) where T : class
        {
            var value = _database.StringGet(key);
            if (value.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(value);
        }

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            _database.StringSet(key, serializedValue, expiry);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }
    }
}
