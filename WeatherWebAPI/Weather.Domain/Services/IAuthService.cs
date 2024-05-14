namespace Weather.Domain.Services;

public interface IAuthService
{
    Task<Response<string>> RegisterUserAsync(RegistrationDto registration);
}
