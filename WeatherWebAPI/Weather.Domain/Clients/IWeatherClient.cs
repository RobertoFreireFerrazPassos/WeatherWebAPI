namespace Weather.Domain.Clients;

public interface IWeatherClient
{
    Task<Response<CityWeatherDto>> GetWeatherAsync(double lat, double lon);
}
