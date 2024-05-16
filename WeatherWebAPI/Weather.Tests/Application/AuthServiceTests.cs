namespace Weather.Tests.Application;

public class AuthServiceTests
{
    private readonly IMapper _mapper;

    private readonly Mock<ICountriesClient> _countriesClientMock = new Mock<ICountriesClient>();

    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

    public AuthServiceTests()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ConfigurationAppMapping());
        });
        _mapper = mockMapper.CreateMapper();
    }


    [Fact]
    public async Task RegisterUserAsync_Should_Return_Success_Response()
    {
        // Arrange
        var authService = new AuthService(_mapper, _countriesClientMock.Object, _userRepositoryMock.Object);

        var registrationDto = new RegistrationDto
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = "user@example.com",
            Password = "123456",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004,01,12),
            PhoneNumber = "+356 22915000",
            LivingCountry = "MLT",
            CitizenCountry = "MLT"
        };

        var countryResponse = new Response<List<CountryDto>>(true, data: new List<CountryDto>
        {
            new CountryDto
            {
                Idd = new IddDto
                {
                    Root = "+3",
                    Suffixes = new string[] { "56", "456" }
                }
            }
        });

        _countriesClientMock.Setup(c => c.GetCountryAsync(It.IsAny<string>())).ReturnsAsync(countryResponse);

        _userRepositoryMock.Setup(u => u.GetByEmailOrUserNameAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Response<UserEntity>(true));

        _userRepositoryMock.Setup(u => u.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(new ResponseWithoutData(true));

        // Act
        var response = await authService.RegisterUserAsync(registrationDto);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeNullOrEmpty();
        response.Data.Should().NotBeNull();
        response.Data.UserName.Should().StartWith("jado");

        _countriesClientMock.Verify(u => u.GetCountryAsync(It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(u => u.GetByEmailOrUserNameAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(u => u.CreateAsync(It.Is<UserEntity>(u =>
            u.Username.StartsWith("jado")
            && !string.IsNullOrWhiteSpace(u.PasswordHash)
        )), Times.Once);
    }
}

