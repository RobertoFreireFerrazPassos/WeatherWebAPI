using Weather.Domain.Clients;

namespace Weather.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;

    private readonly ICountriesClient _countriesClient;

    public AuthService(IMapper mapper, ICountriesClient countriesClient)
    {
        _mapper = mapper;
        _countriesClient = countriesClient;
    }

    public async Task<string> RegisterUser(RegistrationDto registration)
    {
        var user = _mapper.Map<User>(registration);

        var iddRoot = await _countriesClient.GetCountryInfoAsync("MLT");

        user.GenerateName();

        return user.Username;
    }
}
