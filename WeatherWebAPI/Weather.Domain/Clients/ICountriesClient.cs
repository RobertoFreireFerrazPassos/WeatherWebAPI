namespace Weather.Domain.Clients;

public interface ICountriesClient
{
    Task<Response<List<CountryDto>>> GetCountryAsync(string countryCode);
}
