namespace Weather.Application.Interfaces.Security;

public interface IPasswordService
{
    string GeneratePasswordHash(string password);

    bool IsValidPassword(string password, string passwordHash);
}
