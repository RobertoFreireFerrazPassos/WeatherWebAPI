namespace Weather.WebApi;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{ 
    readonly IUserRepository _userRepository;

    public BasicAuthenticationHandler(IUserRepository userRepository,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        _userRepository = userRepository;
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

            var userFromDbResponse = await _userRepository.GetByEmailOrUserNameAsync(string.Empty, username);
            var user = userFromDbResponse.Data;

            if (!userFromDbResponse.IsSuccessful || user is null || !HashUtil.IsValidPassword(password, user.PasswordHash))
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
