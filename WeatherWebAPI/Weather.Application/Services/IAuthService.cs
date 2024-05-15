namespace Weather.Application.Services;

public interface IAuthService
{
    Task<Response<string>> RegisterUserAsync(RegistrationDto registration);
}
