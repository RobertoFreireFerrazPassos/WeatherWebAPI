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

    public async Task<string> GetAsync(string key)
    {
        return await _database.GetStringAsync(key);
    }

    public async Task SetAsync(string key, string value)
    {
        await _database.SetStringAsync(key, value, _options);
    }
}