namespace Weather.CrossCutting.IoC;

public static class NativeDependencyInjector
{
    public static void RegisterServices(this IServiceCollection services, AppConfig appConfig)
    {
        services.AddScoped<IAuthService,AuthService>();
        services.AddScoped<IWeatherService, WeatherService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWeatherRepository, WeatherRepository>();

        RegisterRedisCacheService.Register(services, appConfig.RedisCacheConfig);

        services.AddAutoMapper(typeof(ConfigurationAppMapping));

        services.AddScoped<CacheCountriesMessageHandler>();
        services.AddHttpClient<ICountriesClient, CountriesHttpClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(appConfig.RestCountriesApi.Url))
            .AddHttpMessageHandler<CacheCountriesMessageHandler>()
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddScoped<OpenWeatherMessageHandler>();
        services.AddHttpClient<IWeatherClient, OpenWeatherHttpClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(appConfig.OpenWeather.Url))
            .AddHttpMessageHandler<OpenWeatherMessageHandler>()
            .AddPolicyHandler(GetCircuitBreakerPolicy());
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}