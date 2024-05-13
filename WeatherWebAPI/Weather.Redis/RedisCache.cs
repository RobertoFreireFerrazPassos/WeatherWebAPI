namespace Weather.Redis;

public class RedisCache : ICache
{
    private readonly IDistributedCache _database;

    private readonly DistributedCacheEntryOptions _options;

    public RedisCache(IDistributedCache database, IOptions<RedisCacheConfig> redisCacheConfig)
    {
        _database = database;
        _options = new DistributedCacheEntryOptions
         {
             AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(redisCacheConfig.Value.ExpirationTime)
         };
    }

    public string Get(string key)
    {
        return _database.GetString(key);
    }

    public void Set(string key, string value)
    {
        _database.SetString(key, value, _options);
    }
}