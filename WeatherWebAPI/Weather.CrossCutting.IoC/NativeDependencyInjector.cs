namespace Weather.CrossCutting.IoC;

public static class NativeDependencyInjector
{
    public static void RegisterServices(this IServiceCollection services, AppConfig appConfig)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IWeatherService, WeatherService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWeatherRepository, WeatherRepository>();

        RegisterSecuritiesServices.Register(services);
        RegisterRedisCacheService.Register(services, appConfig.RedisCacheConfig);

        services.AddScoped<CacheCountriesMessageHandler>();
        services.AddHttpClient<ICountriesClient, CountriesHttpClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(appConfig.RestCountriesApi.Url))
            .AddHttpMessageHandler<CacheCountriesMessageHandler>()
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddScoped<CacheOpenWeatherMessageHandler>();
        services.AddHttpClient<IWeatherClient, OpenWeatherHttpClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(appConfig.OpenWeather.Url))
            .AddHttpMessageHandler<CacheOpenWeatherMessageHandler>()
            .AddPolicyHandler(GetCircuitBreakerPolicy());
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}