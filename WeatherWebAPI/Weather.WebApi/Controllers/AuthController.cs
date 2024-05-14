namespace Weather.WebApi.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("registration")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        try
        {
            var result = await _authService.RegisterUserAsync(_mapper.Map<RegistrationDto>(request));

            if (result.IsSuccessful)
            {
                return Ok(new RegistrationResponse()
                {
                    UserName = result.Data,
                });
            }

            return BadRequest(result.ErrorMessage);
        }
        catch (Exception ex)
        {
            //LogError
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login()
    {
        try
        {
            return Ok();
        }
        catch (Exception ex)
        {
            //LogError
            return BadRequest(ex.Message);
        }
    }
}
