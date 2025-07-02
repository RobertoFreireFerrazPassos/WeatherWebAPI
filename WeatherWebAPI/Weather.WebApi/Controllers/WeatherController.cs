namespace Weather.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public class WeatherController(IWeatherService _weatherService) : ControllerBase
{
    [HttpGet]
    [Route("weather")]
    public async Task<IActionResult> Weather()
    {
        var username = HttpContext.User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("missing username");
        }

        var userWeather = await _weatherService.GetWeatherAsync(username);

        return Ok(new UserWeatherResponse()
        {
            HistoricalWeather = userWeather.HistoricalWeather,
            CityWeather = userWeather.CityWeather
        });
    }
}