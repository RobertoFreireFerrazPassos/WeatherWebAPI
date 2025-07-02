namespace Weather.Application.Interfaces.Services;

public interface IWeatherService
{
    Task<UserWeatherDto> GetWeatherAsync(string username);
}