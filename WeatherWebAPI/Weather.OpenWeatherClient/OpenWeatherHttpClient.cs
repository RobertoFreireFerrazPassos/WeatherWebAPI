namespace Weather.OpenWeatherClient;

public class OpenWeatherHttpClient : BaseHttpClient, IWeatherClient
{
    public readonly string _appId;

    public OpenWeatherHttpClient(HttpClient httpClient, IOptions<ApiConfig> apiConfig) : base(httpClient)
    {
        _appId = apiConfig.Value.Key;
    }

    public async Task<Response<CityWeather>> GetWeatherAsync(double lat, double lon)
    {
        return await GetAsync<CityWeather>($"?{OpenWeatherConstants.LatCodeParameter}={lat}&{OpenWeatherConstants.LngCodeParameter}={lon}&APPID={_appId}");
    }
}