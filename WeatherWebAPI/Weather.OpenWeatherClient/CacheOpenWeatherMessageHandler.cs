namespace Weather.OpenWeatherClient;

public class CacheOpenWeatherMessageHandler : DelegatingHandler
{
    private readonly ICache _cache;

    public CacheOpenWeatherMessageHandler(ICache cache)
    {
        _cache = cache;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
        var latKey = query[OpenWeatherConstants.LatCodeParameter];
        var lngKey = query[OpenWeatherConstants.LngCodeParameter];
        var cacheKey = OpenWeatherConstants.CacheKey + latKey + lngKey;
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