namespace Weather.Application.Services;

public class AuthService(
    IMapper _mapper, 
    ICountriesClient _countriesClient, 
    IUserRepository _userRepository,
    IPasswordService _passwordService) : IAuthService
{
    public async Task<UserDto> RegisterUserAsync(RegistrationDto registration)
    {
        var user = _mapper.Map<UserEntity>(registration);

        var countries = await _countriesClient.GetCountryAsync(user.GetLocation());

        var idd = countries[0].Idd;

        if (idd.Suffixes.All(suffix => !user.IsValidPhoneNumber(idd.Root + suffix)))
        {
            throw new Exception ("Phone number is not valid for the user living country");
        }

        user.PasswordHash = _passwordService.GeneratePasswordHash(registration.Password);
        user.GenerateName();

        var userFromDb = await _userRepository.GetByEmailOrUserNameAsync(user.Email, user.Username);

        if (userFromDb is not null)
        {
            throw new Exception("A user with same email or username already exists");
        }

        await _userRepository.CreateAsync(user);

        return new UserDto()
        {
            Username = user.Username,
        };
    }
}
