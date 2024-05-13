namespace Weather.Domain.Clients;

public interface ICountriesClient
{
    Task<string> GetCountryInfoAsync(string countryCode);
}
