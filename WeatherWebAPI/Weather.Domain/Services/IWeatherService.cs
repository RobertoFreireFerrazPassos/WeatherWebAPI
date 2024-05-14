namespace Weather.Domain.Services;

public interface IWeatherService
{
    Task<Response<bool>> GetWeatherAsync(string username);
}