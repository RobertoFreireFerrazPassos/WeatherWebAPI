namespace Weather.CountriesClient;

public class CountriesHttpClient : BaseHttpClient, ICountriesClient
{
    public CountriesHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<Response<List<Country>>> GetCountryAsync(string countryCode)
    {
        return await GetAsync<List<Country>>($"?{CountriesClientConstants.CountryCodeParameter}={countryCode}");
    }
}