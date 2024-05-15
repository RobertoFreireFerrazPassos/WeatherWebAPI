namespace Weather.Tests.WebApi.Controllers;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherService> _weatherServiceMock = new Mock<IWeatherService>();

    [Fact]
    public async Task GetWeather_Should_ReturnStatus200AndExpectedProduct()
    {
        // Arrange
        var weatherController = new WeatherController(_weatherServiceMock.Object);
        weatherController.ControllerContext = new ControllerContext();
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "jado47744")
        }));
        weatherController.ControllerContext.HttpContext = httpContext;
        var expectedResponse = new Response<UserWeatherResponse>(true, data : new UserWeatherResponse());

        _weatherServiceMock
            .Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await weatherController.Weather() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().Be(expectedResponse.Data);
    }
}