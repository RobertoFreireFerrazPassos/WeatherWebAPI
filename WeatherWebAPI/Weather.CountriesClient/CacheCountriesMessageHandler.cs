namespace Weather.CountriesClient;

public class CacheCountriesMessageHandler : DelegatingHandler
{
    private readonly ICache _cache;

    public CacheCountriesMessageHandler(ICache cache)
    {
        _cache = cache;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
        var key = query[CountriesClientConstants.CountryCodeParameter];
        var cacheKey = CountriesClientConstants.CacheKey + key;
        var cacheResponse = await _cache.Get(cacheKey);

        if (string.IsNullOrWhiteSpace(cacheResponse))
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseAsSring = await response.Content.ReadAsStringAsync();

                await _cache.Set(cacheKey, responseAsSring);
            }

            return response;
        }

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cacheResponse, Encoding.UTF8, "application/json")
        };
    }
}