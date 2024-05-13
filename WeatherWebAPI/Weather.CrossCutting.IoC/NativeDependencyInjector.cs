namespace Weather.CrossCutting.IoC;

public static class NativeDependencyInjector
{
    public static void RegisterServices(this IServiceCollection services, AppConfig appConfig)
    {
        services.AddScoped<IAuthService,AuthService>();

        RegisterRedisCacheService.Register(services, appConfig.RedisCacheConfig);

        services.AddAutoMapper(typeof(ConfigurationAppMapping));
    }
}