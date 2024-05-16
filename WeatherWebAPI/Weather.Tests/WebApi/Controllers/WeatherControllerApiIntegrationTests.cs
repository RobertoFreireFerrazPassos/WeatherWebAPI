namespace Weather.Tests.WebApi.Controllers;

public class WeatherControllerApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherControllerApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = WebApplicationFactoryUtil.SetWebApplicationFactory(factory);
    }
}