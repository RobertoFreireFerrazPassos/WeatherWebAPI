namespace Weather.Application.Services;

public interface IWeatherService
{
    Task<Response<UserWeatherResponse>> GetWeatherAsync(string username);
}