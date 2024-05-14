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
        try
        {
            return _database.GetString(key);
        }
        catch (Exception ex)
        {
            //LogError
            return string.Empty;
        }
    }

    public void Set(string key, string value)
    {
        try
        {
            _database.SetString(key, value, _options);
        }
        catch (Exception ex)
        {
            //LogError
            return;
        }
    }
}