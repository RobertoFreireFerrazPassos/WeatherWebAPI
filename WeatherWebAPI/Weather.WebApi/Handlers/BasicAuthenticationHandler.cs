namespace Weather.WebApi.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    readonly IUserRepository _userRepository;
    readonly IPasswordService _passwordService;

    public BasicAuthenticationHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string username = null;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
            username = credentials.FirstOrDefault();
            var password = credentials.LastOrDefault();

            var userFromDb = await _userRepository.GetByEmailOrUserNameAsync(string.Empty, username);

            if (userFromDb is null || !_passwordService.IsValidPassword(password, userFromDb.PasswordHash))
            {
                throw new ArgumentException("Invalid credentials");
            }
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
        }

        var claims = new[] {
                new Claim(ClaimTypes.Name, username)
            };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
