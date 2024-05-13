namespace Weather.WebApi.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly ICache _cache;

    public AuthController(ICache cache)
    {
        _cache = cache;
    }
    
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        _cache.Set("1234","VAlueforKey123");
        return Ok(_cache.Get("123"));
    }
}
