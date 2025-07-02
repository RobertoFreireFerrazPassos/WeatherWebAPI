namespace Weather.Security;

public static class RegisterSecuritiesServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IPasswordService, PasswordService>();
    }
}