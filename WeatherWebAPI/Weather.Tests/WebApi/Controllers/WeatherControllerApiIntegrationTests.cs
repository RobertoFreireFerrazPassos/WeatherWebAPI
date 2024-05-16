namespace Weather.Tests.WebApi.Controllers;

public class WeatherControllerApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherControllerApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = WebApplicationFactoryUtil.SetWebApplicationFactory(factory);
    }

    public async Task WeatherEndpoint_Should_ReturnSuccessAndCorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/weather");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
    }
}