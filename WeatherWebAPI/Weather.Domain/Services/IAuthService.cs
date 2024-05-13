namespace Weather.Domain.Services;

public interface IAuthService
{
    Task<string> RegisterUser(string fullname, string password);
}
