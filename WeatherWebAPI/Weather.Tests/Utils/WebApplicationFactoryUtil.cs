namespace Weather.Tests.Utils;

internal static class WebApplicationFactoryUtil
{
    public static WebApplicationFactory<Program> SetWebApplicationFactory(WebApplicationFactory<Program> factory)
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json")
                      .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)
                      .AddEnvironmentVariables()
                      .AddInMemoryCollection(new Dictionary<string, string>
                      {
                          ["Configuration:WeatherDb:ConnectionString"] = TestConstants.ConnectionString
                      });
            });
        });
    }
}