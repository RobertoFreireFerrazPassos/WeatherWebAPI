namespace Weather.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;

    public AuthService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<string> RegisterUser(RegistrationDto registration)
    {
        var user = _mapper.Map<User>(registration);

        user.GenerateName();

        return user.Username;
    }
}
