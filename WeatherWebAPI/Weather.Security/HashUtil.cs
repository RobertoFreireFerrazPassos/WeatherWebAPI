namespace Weather.Security;

public static class HashUtil
{
    public static (string PasswordHash, string Salt) GenerateHashPasswordAndSalt(string password)
    {
        var salt = GenerateSalt();
        var passwordHash = HashPassword(password+ salt);

        return (passwordHash, salt);
    }

    public static bool IsValidPassword(string password, string salt, string passwordHash)
    {
        return passwordHash == HashPassword(password + salt);
    }

    private static string GenerateSalt()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var saltBuilder = new StringBuilder();

        for (int i = 0; i < 5; i++)
        {
            saltBuilder.Append(chars[random.Next(chars.Length)]);
        }

        return saltBuilder.ToString();
    }
}