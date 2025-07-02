namespace Weather.Application.Interfaces.Clients;

public interface ICountriesClient
{
    Task<List<CountryDto>> GetCountryAsync(string countryCode);
}
