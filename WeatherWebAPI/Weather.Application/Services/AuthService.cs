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

    public async Task<Response<string>> RegisterUser(RegistrationDto registration)
    {
        var user = _mapper.Map<User>(registration);

        var countryResponse = await _countriesClient.GetCountryAsync(user.LivingCountry);

        if (!countryResponse.IsSuccessful)
        {
            return new Response<string>(countryResponse.IsSuccessful, countryResponse.ErrorMessage);
        }

        var idd = countryResponse.Data[0].Idd;

        if (idd.Suffixes.All(suffix => !user.IsValidPhoneNumber(idd.Root + suffix)))
        {
            return new Response<string>(false, "Phone number is not valid for the user living country");
        }

        var (passwordHash, salt) = HashUtil.GenerateHashPasswordAndSalt(registration.Password);

        user.PasswordHash = passwordHash;
        user.Salt = salt;
        user.GenerateName();

        var userFromDbResponse = await _userRepository.GetByEmailOrUserNameAsync(user.Email, user.Username);

        if (!userFromDbResponse.IsSuccessful)
        {
            return new Response<string>(false, userFromDbResponse.ErrorMessage);
        }

        if (userFromDbResponse.Data is not null)
        {
            return new Response<string>(false, "A user with same email or username already exists");
        }

        var createUserResponse = await _userRepository.CreateAsync(user);

        if (!createUserResponse.IsSuccessful)
        {
            return new Response<string>(false, createUserResponse.ErrorMessage);
        }

        return new Response<string>(countryResponse.IsSuccessful, string.Empty, user.Username);
    }
}
