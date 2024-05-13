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
            var userName = await _authService.RegisterUser(_mapper.Map<RegistrationDto>(request));

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
