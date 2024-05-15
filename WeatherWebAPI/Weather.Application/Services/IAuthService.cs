namespace Weather.Application.Services;

public interface IAuthService
{
    Task<Response<RegistrationResponse>> RegisterUserAsync(RegistrationDto registration);
}
