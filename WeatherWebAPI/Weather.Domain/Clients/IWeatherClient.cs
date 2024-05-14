namespace Weather.Domain.Clients;

public interface IWeatherClient
{
    Task<Response<CityWeather>> GetWeatherAsync(double lat, double lon);
}
