namespace Weather.Tests.WebApi.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authService = new Mock<IAuthService>();

    private readonly IMapper _mapper;

    public AuthControllerTests()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ConfigurationAppMapping());
        });
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task Register_Should_ReturnOk()
    {
        // Arrange
        var userName = "jado47744";
        var request = new RegistrationRequest()
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
        var expectedResponse = new Response<RegistrationResponse>(true, data: new RegistrationResponse() { UserName = userName });
        _authService
            .Setup(s => s.RegisterUserAsync(It.IsAny<RegistrationDto>()))
            .ReturnsAsync(expectedResponse);
        var authController = new AuthController(_authService.Object, _mapper);

        // Act
        var result = await authController.Register(request) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().Be(expectedResponse.Data);
    }
}