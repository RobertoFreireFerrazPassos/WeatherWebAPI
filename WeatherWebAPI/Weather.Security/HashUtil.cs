namespace Weather.Security;

public static class HashUtil
{
    public static string GeneratePasswordHash(string password)
    {
        return HashPassword(password);
    }

    public static bool IsValidPassword(string password, string passwordHash)
    {
        return Verify(password, passwordHash);
    }
}