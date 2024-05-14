namespace Weather.CrossCutting.IoC;

public static class NativeDependencyInjector
{
    public static void RegisterServices(this IServiceCollection services, AppConfig appConfig)
    {
        services.AddScoped<IAuthService,AuthService>();
        services.AddScoped<IWeatherService, WeatherService>();

        services.AddScoped<IUserRepository, UserRepository>();

        RegisterRedisCacheService.Register(services, appConfig.RedisCacheConfig);

        services.AddAutoMapper(typeof(ConfigurationAppMapping));

        services.AddScoped<CacheCountriesMessageHandler>();
        services.AddHttpClient<ICountriesClient, CountriesHttpClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(appConfig.RestCountriesApi.Url))
            .AddHttpMessageHandler<CacheCountriesMessageHandler>()
            .AddPolicyHandler(GetCircuitBreakerPolicy());
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}