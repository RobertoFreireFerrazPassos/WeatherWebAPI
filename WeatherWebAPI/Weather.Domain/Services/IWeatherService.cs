namespace Weather.Domain.Services;

public interface IWeatherService
{
    Task<Response<CityWeather>> GetWeatherAsync(string username);
}