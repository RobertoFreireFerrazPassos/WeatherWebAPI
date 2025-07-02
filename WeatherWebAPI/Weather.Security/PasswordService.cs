namespace Weather.Security;

public class PasswordService : IPasswordService
{
    public string GeneratePasswordHash(string password)
    {
        return HashPassword(password);
    }

    public bool IsValidPassword(string password, string passwordHash)
    {
        return Verify(password, passwordHash);
    }
}