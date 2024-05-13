namespace Weather.CrossCutting.IoC;

public static class NativeDependencyInjector
{
    public static void RegisterServices(this IServiceCollection services, AppConfig appConfig)
    {
        RegisterRedisCacheService.Register(services, appConfig.RedisCacheConfig);
    }
}