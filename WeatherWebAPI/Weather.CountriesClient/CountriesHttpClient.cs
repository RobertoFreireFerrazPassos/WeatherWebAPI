namespace Weather.CountriesClient;

public class CountriesHttpClient : BaseHttpClient, ICountriesClient
{
    public CountriesHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<List<CountryDto>> GetCountryAsync(string countryCode)
    {
        return await GetAsync<List<CountryDto>>($"?{CountriesClientConstants.CountryCodeParameter}={countryCode}");
    }
}