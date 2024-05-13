namespace Weather.WebApi.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return Ok();
    }
}
