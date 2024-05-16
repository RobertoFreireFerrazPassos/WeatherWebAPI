namespace Weather.Tests.Infra.Redis;

public class RedisCacheIntegrationTests
{
    private readonly Mock<IOptions<RedisCacheConfig>> _dbConfigMock = new Mock<IOptions<RedisCacheConfig>>();

    private readonly RedisCacheConfig _redisCacheConfig = new RedisCacheConfig()
    {
        ExpirationTime = 60
    };

    private RedisCache _redisCache {  get; set; }

    private ServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddDistributedRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "RedisCacheInstance";
        });

        return services.BuildServiceProvider();
    }

    public RedisCacheIntegrationTests()
    {
        var _serviceProvider = ConfigureServices();
        var database = _serviceProvider.GetService<IDistributedCache>();
        _dbConfigMock.Setup(m => m.Value).Returns(_redisCacheConfig);
        _redisCache = new RedisCache(database, _dbConfigMock.Object);
    }

    [Fact]
    public async Task Should_SetAndGetValueInRedis()
    {
        var key = "integrationTestKey123";
        var value = "integrationTestValue123";
        // Act
        await _redisCache.SetAsync(key, value);
        var response = await _redisCache.GetAsync(key);

        // Assert
        response.Should().Be(value);
    }
}