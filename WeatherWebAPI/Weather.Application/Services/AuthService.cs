namespace Weather.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;

    private readonly ICountriesClient _countriesClient;

    private readonly IUserRepository _userRepository;

    public AuthService(IMapper mapper, ICountriesClient countriesClient, IUserRepository userRepository)
    {
        _mapper = mapper;
        _countriesClient = countriesClient;
        _userRepository = userRepository;
    }

    public async Task<Response<RegistrationResponse>> RegisterUserAsync(RegistrationDto registration)
    {
        var user = _mapper.Map<UserEntity>(registration);

        var countryResponse = await _countriesClient.GetCountryAsync(user.GetLocation());

        if (!countryResponse.IsSuccessful)
        {
            return new Response<RegistrationResponse>(countryResponse.IsSuccessful, countryResponse.ErrorMessage);
        }

        var idd = countryResponse.Data[0].Idd;

        if (idd.Suffixes.All(suffix => !user.IsValidPhoneNumber(idd.Root + suffix)))
        {
            return new Response<RegistrationResponse>(false, "Phone number is not valid for the user living country");
        }

        user.PasswordHash = HashUtil.GeneratePasswordHash(registration.Password);
        user.GenerateName();

        var userFromDbResponse = await _userRepository.GetByEmailOrUserNameAsync(user.Email, user.Username);

        if (!userFromDbResponse.IsSuccessful)
        {
            return new Response<RegistrationResponse>(userFromDbResponse.IsSuccessful, userFromDbResponse.ErrorMessage);
        }

        if (userFromDbResponse.Data is not null)
        {
            return new Response<RegistrationResponse>(false, "A user with same email or username already exists");
        }

        var createUserResponse = await _userRepository.CreateAsync(user);

        if (!createUserResponse.IsSuccessful)
        {
            return new Response<RegistrationResponse>(createUserResponse.IsSuccessful, createUserResponse.ErrorMessage);
        }

        return new Response<RegistrationResponse>(true, data : new RegistrationResponse()
        {
            UserName = user.Username,
        });
    }
}
