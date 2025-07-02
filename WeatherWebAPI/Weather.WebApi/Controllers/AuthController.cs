namespace Weather.WebApi.Controllers;

[ApiController]
[Route("api")]
public class AuthController(
    IAuthService _authService, 
    IMapper _mapper) : ControllerBase
{
    [HttpPost]
    [Route("registration")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        var user = await _authService.RegisterUserAsync(_mapper.Map<RegistrationDto>(request));

        return Ok(new RegistrationResponse()
        {
            UserName = user.Username
        });
    }
}
