namespace Weather.Domain.Clients;

public interface ICountriesClient
{
    Task<Response<List<Country>>> GetCountryAsync(string countryCode);
}
