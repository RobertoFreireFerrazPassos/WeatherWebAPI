namespace Weather.Redis;

public class RedisCacheConfig
{
    public string ConnectionString { get; set; }

    public double ExpirationTime { get; set; }

    public string InstanceName { get; set; }
}