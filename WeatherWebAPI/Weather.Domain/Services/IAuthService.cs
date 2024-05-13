namespace Weather.Domain.Services;

public interface IAuthService
{
    Task<string> RegisterUser(RegistrationDto registration);
}
