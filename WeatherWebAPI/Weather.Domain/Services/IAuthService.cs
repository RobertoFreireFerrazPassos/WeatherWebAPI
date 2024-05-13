namespace Weather.Domain.Services;

public interface IAuthService
{
    Task<Response<string>> RegisterUser(RegistrationDto registration);
}
