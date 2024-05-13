namespace Weather.Application.Services;

public class AuthService : IAuthService
{
    public async Task<string> RegisterUser(string fullName, string password)
    {
        return GenerateUsername(fullName);
    }

    private string GenerateUsername(string fullName)
    {
        var names = fullName.Split(' ');
        var username = string.Empty;

        username += names[0].Substring(0, 2).ToLower();
        username += names[1].Substring(0, 2).ToLower();

        Random random = new Random();
        username += random.Next(10000, 99999);

        return username;
    }
}
