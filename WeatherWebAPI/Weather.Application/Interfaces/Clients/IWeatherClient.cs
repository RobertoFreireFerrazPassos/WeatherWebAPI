namespace Weather.Application.Interfaces.Clients;

public interface IWeatherClient
{
    Task<CityWeatherDto> GetWeatherAsync(double lat, double lon);
}
