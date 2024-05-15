namespace Weather.Tests.WebApi.Controllers;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherService> _weatherServiceMock = new Mock<IWeatherService>();

    private WeatherController GetWeatherController(string userName)
    {
        var weatherController = new WeatherController(_weatherServiceMock.Object);
        weatherController.ControllerContext = new ControllerContext();
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, userName)
        }));
        weatherController.ControllerContext.HttpContext = httpContext;

        return weatherController;
    }

    [Fact]
    public async Task GetWeather_Should_ReturnOk()
    {
        // Arrange
        var expectedResponse = new Response<UserWeatherResponse>(true, data : new UserWeatherResponse());
        _weatherServiceMock
            .Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedResponse);
        var weatherController = GetWeatherController("jado47744");

        // Act
        var result = await weatherController.Weather() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().Be(expectedResponse.Data);
    }

    [Fact]
    public async Task When_MissingUsername_GetWeather_Should_ReturnBadRequest()
    {
        // Arrange
        var expectedResponse = new Response<UserWeatherResponse>(true, data: new UserWeatherResponse());
        _weatherServiceMock
            .Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedResponse);
        var weatherController = GetWeatherController(string.Empty);

        // Act
        var result = await weatherController.Weather() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(400);
        result.Value.Should().Be("missing username");
    }

    [Fact]
    public async Task When_GetWeather_Throws_Exception_Returns_BadRequest()
    {
        // Arrange
        var exceptionMessage = "An error occurred while retrieving weather data.";
        _weatherServiceMock.Setup(x => x.GetWeatherAsync("jado47744"))
                          .ThrowsAsync(new Exception(exceptionMessage));
        var weatherController = GetWeatherController("jado47744");

        // Act
        var result = await weatherController.Weather() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(400);
        result.Value.Should().Be(exceptionMessage);
    }
}