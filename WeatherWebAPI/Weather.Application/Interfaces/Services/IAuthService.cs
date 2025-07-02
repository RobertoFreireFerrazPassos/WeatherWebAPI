namespace Weather.Application.Interfaces.Services;

public interface IAuthService
{
    Task<UserDto> RegisterUserAsync(RegistrationDto registration);
}