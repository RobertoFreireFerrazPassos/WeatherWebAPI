namespace Weather.WebApi.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("registration")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        try
        {
            var validRequest = request.IsValid();

            if (!validRequest.IsValid)
            {
                return BadRequest(validRequest.ErrorMessage);
            }

            var userName = await _authService.RegisterUser(request.FullName, request.Password);

            return Ok(new RegistrationResponse()
            {
                UserName = userName,
            });
        }
        catch (Exception ex)
        {
            //_log.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
