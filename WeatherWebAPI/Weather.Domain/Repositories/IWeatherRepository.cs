namespace Weather.Domain.Repositories;

public interface IWeatherRepository
{
    Task<Response<IEnumerable<WeatherEntity>>> GetByCountryCodeAsync(string countryCode);
}
