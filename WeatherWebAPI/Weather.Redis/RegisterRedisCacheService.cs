namespace Weather.Redis;

public class RegisterRedisCacheService
{
    public static void Register(IServiceCollection services, RedisCacheConfig redisCacheConfig)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisCacheConfig.ConnectionString;
            options.InstanceName = redisCacheConfig.InstanceName;
        });

        services.AddSingleton<ICache, RedisCache>();
    }
}
