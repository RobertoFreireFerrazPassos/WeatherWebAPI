using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using Weather.Application.DataContracts.Requests;

namespace Weather.Tests.WebApi.Controllers;

public class AuthControllerApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthControllerApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Replace "YourConnectionString" with your actual connection string
                config.AddJsonFile("appsettings.json")
                      .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)
                      .AddEnvironmentVariables()
                      .AddInMemoryCollection(new Dictionary<string, string>
                      {
                          ["Configuration:WeatherDb:ConnectionString"] = "Host=localhost; Port=8082; Database=weatherit; Username=simha; Password=Postgres2019!;"
                      });
            });
        });
    }

    [Fact]
    public async Task RegisterEndpoint_Should_ReturnSuccessAndCorrectContentType()
    {
        // Arrange
        var input = new RegistrationRequest()
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = "user123@example.com",
            Password = "123456",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004,01,12),
            PhoneNumber = "+356 22915000",
            LivingCountry = "MLT",
            CitizenCountry = "MLT"
        };
        var request = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/registration", request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
    }
}