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

    public async Task<Response<string>> RegisterUser(RegistrationDto registration)
    {
        var user = _mapper.Map<User>(registration);

        var countryResponse = await _countriesClient.GetCountryAsync(user.LivingCountry);

        if (!countryResponse.IsSuccessful)
        {
            return new Response<string>(countryResponse.IsSuccessful, countryResponse.ErrorMessage);
        }

        var idd = countryResponse.Data[0].Idd;

        if (!user.IsValidPhoneNumber(idd.Root + idd.Suffixes[0]))
        {
            return new Response<string>(false, "Phone number is not valid for the user living country");
        }

        user.GenerateName();

        return new Response<string>(countryResponse.IsSuccessful, string.Empty, user.Username);
    }
}
