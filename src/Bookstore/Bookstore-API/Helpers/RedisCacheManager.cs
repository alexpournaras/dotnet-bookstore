using StackExchange.Redis;
using System.Text.Json;

namespace BookstoreAPI.Helpers
{
    public class RedisCacheManager
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCacheManager(IConfiguration configuration)
        {
            var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
            
            _redis = ConnectionMultiplexer.Connect(options);
            _database = _redis.GetDatabase(options.DefaultDatabase.GetValueOrDefault());
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
