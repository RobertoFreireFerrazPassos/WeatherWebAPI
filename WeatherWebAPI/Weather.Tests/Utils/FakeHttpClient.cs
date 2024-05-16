namespace Weather.Tests.Utils;

internal static class FakeHttpClient
{
    public static HttpClient GenerateFakeHttpClient(
        string stringContent,
        HttpStatusCode httpStatusCode,
        string baseAddress = "https://example.com")
    {
        return new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(httpStatusCode)
        {
            Content = new StringContent(stringContent)
        }))
        {
            BaseAddress = new Uri(baseAddress)
        };
    }
}