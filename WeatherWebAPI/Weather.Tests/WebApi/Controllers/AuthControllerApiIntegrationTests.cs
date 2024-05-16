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

    private async Task CleanDatabaseAsync()
    {
        var dbConfigMock = new Mock<IOptions<DbConfig>>();
        dbConfigMock.Setup(m => m.Value).Returns(new DbConfig()
        {
            ConnectionString = "Host=localhost; Port=8082; Database=weatherit; Username=simha; Password=Postgres2019!;"
        });
        var repository = new Repository(dbConfigMock.Object);

        var sql = @"
            DELETE FROM UserRegistration
            WHERE Email = 'userTeste123@example.com'; 
        ";

        await repository.ExecuteAsync(sql, new { });
    }

    private RegistrationRequest GetRegistrationRequest()
    {
        return new RegistrationRequest()
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = "userTeste123@example.com",
            Password = "123456",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004, 01, 12),
            PhoneNumber = "+356 22915000",
            LivingCountry = "MLT",
            CitizenCountry = "MLT"
        };
    }

    [Fact]
    public async Task RegisterEndpoint_Should_ReturnSuccessAndCorrectContentType()
    {
        // Arrange
        await CleanDatabaseAsync();

        var input = GetRegistrationRequest();
        var request = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/registration", request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
    }

    [Theory]
    [InlineData("Firstname","", "First name is required")]
    [InlineData("Firstname", "1","First name can not be shorter than 2 characters")]
    [InlineData("Firstname", "jashdkjashdkajshdjkashdjkahsjkdhasjkdhajkshdjkahdkajsdhkadjashjkadhskajhdkjashdkajshdjka", "First name can not be longer than 30 characters")]
    [InlineData("Lastname", "", "Last name is required")]
    [InlineData("Lastname", "1", "Last name can not be shorter than 2 characters")]
    [InlineData("Lastname", "jashdkjashdkajshdjkashdjkahsjkdhasjkdhajkshdjkahdkajsdhkadjashjkadhskajhdkjashdkajshdjkajashdkjashdkajshdjkashdjkahsjkdhasjkdhajkshdjkahdkajsdhkadjashjkadhskajhdkjashdkajshdjkajashdkjashdkajshdjkashdjkahsjkdhasjkdhajkshdjkahdkajsdhkadjashjkadhskajhdkjashdkajshdjka", "Last name can not be longer than 100 characters")]
    [InlineData("Email", "", "Email is required")]
    [InlineData("Email", "invalidEmail", "Email is invalid")]
    [InlineData("Password", "", "Password is required")]
    [InlineData("Password", "12345", "Password can not be shorter than 6 characters")]
    [InlineData("Password", "passwordpasswordpasswordpassword", "Password can not be longer than 20 characters")]
    [InlineData("PhoneNumber", "", "Phone number is required")]
    [InlineData("PhoneNumber", "12345", "Phone number can not be shorter than 6 characters")]
    [InlineData("LivingCountry", "", "Living country is required")]
    [InlineData("CitizenCountry", "", "Citizen country is required")]
    public async Task When_InvalidRequest_RegisterEndpoint_Should_ReturnErrorMessage(string propertyName, string propertyValue, string errorMessage)
    {
        // Arrange
        await CleanDatabaseAsync();

        var input = GetRegistrationRequest();
        var propertyInfo = typeof(RegistrationRequest).GetProperty(propertyName);
        propertyInfo.SetValue(input, propertyValue);
        var request = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/registration", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStringAsync();
        var responseObj = JsonConvert.DeserializeObject<ValidationErrorResponse>(responseBody);

        responseObj.Should().NotBeNull();
        responseObj.Errors.Should().NotBeNull();
        responseObj.Errors.Should().ContainKey(propertyName);
        responseObj.Errors[propertyName].Should().Contain(errorMessage);
    }
}