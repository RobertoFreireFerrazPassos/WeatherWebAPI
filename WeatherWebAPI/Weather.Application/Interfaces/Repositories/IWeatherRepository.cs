namespace Weather.Application.Interfaces.Repositories;

public interface IWeatherRepository
{
    Task<IEnumerable<WeatherEntity>> GetByCountryCodeAsync(string countryCode);
}
