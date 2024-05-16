namespace Weather.Tests.Infra.BaseClient;

public class BaseHttpClientTests
{
    [Fact]
    public async Task When_ValidResponse_GetAsync_Should_ReturnsData()
    {
        // Arrange
        var path = "/api/weather/123";
        var expectedValue = "Value123";
        var httpClient = FakeHttpClient.GenerateFakeHttpClient($"{{\"key\":\"{expectedValue}\"}}", HttpStatusCode.OK);
        var baseHttpClient = new BaseHttpClient(httpClient);

        // Act
        var response = await baseHttpClient.GetAsync<DataForRequest>(path);

        // Assert
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeEmpty();
        response.Data.Should().NotBeNull();
        response.Data.Key.Should().Be(expectedValue);
    }

    [Fact]
    public async Task When_InvalidResponse_GetAsync_Should_ReturnsErrorMessage()
    {
        // Arrange
        var path = "/api/weather/123";
        var httpClient = FakeHttpClient.GenerateFakeHttpClient(string.Empty, HttpStatusCode.BadRequest);
        var baseHttpClient = new BaseHttpClient(httpClient);

        // Act
        var response = await baseHttpClient.GetAsync<DataForRequest>(path);

        // Assert
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be("Error during request");
        response.Data.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData($"{{\"IntKey\":\"13\"}}")]
    public async Task When_InvalidObjectResponse_GetAsync_Should_ReturnsErrorMessage(string stringContent)
    {
        // Arrange
        var path = "/api/weather/123";
        var httpClient = FakeHttpClient.GenerateFakeHttpClient(stringContent, HttpStatusCode.OK);
        var baseHttpClient = new BaseHttpClient(httpClient);

        // Act
        var response = await baseHttpClient.GetAsync<DataForRequest>(path);

        // Assert
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be("Exception during request");
        response.Data.Should().BeNull();
    }

    private class DataForRequest
    {
        public string Key { get; set; }

        public int IntKey { get; set; }
    }
}
