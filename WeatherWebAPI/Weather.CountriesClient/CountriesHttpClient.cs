namespace Weather.CountriesClient;

public class CountriesHttpClient : BaseHttpClient, ICountriesClient
{
    public CountriesHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<string> GetCountryInfoAsync(string countryCode)
    {
        var country = await GetAsync<List<Country>>($"?codes={countryCode}");

        return country[0].Idd.Root;
    }
}