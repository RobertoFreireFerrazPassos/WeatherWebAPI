namespace Weather.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    
    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet]
    [Route("weather")]
    public async Task<IActionResult> Weather()
    {
        try
        {
            var username = HttpContext.User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("missing username");
            }

            var result = await _weatherService.GetWeatherAsync(username);

            if (result.IsSuccessful)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }
        catch (Exception ex)
        {
            //LogError
            return BadRequest(ex.Message);
        }
    }
}
