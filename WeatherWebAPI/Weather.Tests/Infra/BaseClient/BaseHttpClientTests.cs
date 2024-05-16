namespace Weather.Tests.Infra.BaseClient;

public class BaseHttpClientTests
{
    [Fact]
    public async Task GetAsync_ValidResponse_ReturnsData()
    {
        // Arrange
        var path = "/api/weather/123";
        var expectedValue = "Value123";
        var expectedResponse = new DataForRequest() { Key = expectedValue };
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
    public async Task GetAsync_InvalidResponse_ReturnsErrorMessage()
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

    private class DataForRequest
    {
        public string Key { get; set; }
    }
}
