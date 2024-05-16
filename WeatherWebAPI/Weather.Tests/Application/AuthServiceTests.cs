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
        var location = "MLT";
        var email = "user@example.com";
        var registrationDto = new RegistrationDto
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = email,
            Password = "123456",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004,01,12),
            PhoneNumber = "+356 22915000",
            LivingCountry = location,
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

        _countriesClientMock.Setup(c => c.GetCountryAsync(location)).ReturnsAsync(countryResponse);
        _userRepositoryMock.Setup(u => u.GetByEmailOrUserNameAsync(email, It.IsAny<string>())).ReturnsAsync(new Response<UserEntity>(true));
        _userRepositoryMock.Setup(u => u.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(new ResponseWithoutData(true));

        // Act
        var response = await authService.RegisterUserAsync(registrationDto);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeNullOrEmpty();
        response.Data.Should().NotBeNull();
        response.Data.UserName.Should().StartWith("jado");

        _countriesClientMock.Verify(u => u.GetCountryAsync(location), Times.Once);
        _userRepositoryMock.Verify(u => u.GetByEmailOrUserNameAsync(email, It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(u => u.CreateAsync(It.Is<UserEntity>(u =>
            u.Username.StartsWith("jado")
            && !string.IsNullOrWhiteSpace(u.PasswordHash)
        )), Times.Once);
    }

    [Fact]
    public async Task When_InvalidPhoneNumber_RegisterUserAsync_Should_Return_ErrorMessage()
    {
        // Arrange
        var authService = new AuthService(_mapper, _countriesClientMock.Object, _userRepositoryMock.Object);
        var location = "MLT";
        var email = "user@example.com";
        var registrationDto = new RegistrationDto
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = email,
            Password = "123456",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004, 01, 12),
            PhoneNumber = "+356 22915000",
            LivingCountry = location,
            CitizenCountry = "MLT"
        };

        var countryResponse = new Response<List<CountryDto>>(true, data: new List<CountryDto>
        {
            new CountryDto
            {
                Idd = new IddDto
                {
                    Root = "+1",
                    Suffixes = new string[] { "56", "456" }
                }
            }
        });

        _countriesClientMock.Setup(c => c.GetCountryAsync(location)).ReturnsAsync(countryResponse);
        _userRepositoryMock.Setup(u => u.GetByEmailOrUserNameAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Response<UserEntity>(true));
        _userRepositoryMock.Setup(u => u.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(new ResponseWithoutData(true));

        // Act
        var response = await authService.RegisterUserAsync(registrationDto);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be("Phone number is not valid for the user living country");
        response.Data.Should().BeNull();

        _countriesClientMock.Verify(u => u.GetCountryAsync(location), Times.Once);
        _userRepositoryMock.Verify(u => u.GetByEmailOrUserNameAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _userRepositoryMock.Verify(u => u.CreateAsync(It.IsAny<UserEntity>()), Times.Never);
    }

    [Fact]
    public async Task When_SameUser_RegisterUserAsync_Should_Return_ErrorMessage()
    {
        // Arrange
        var authService = new AuthService(_mapper, _countriesClientMock.Object, _userRepositoryMock.Object);
        var location = "MLT";
        var email = "user@example.com";
        var registrationDto = new RegistrationDto
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = email,
            Password = "123456",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004, 01, 12),
            PhoneNumber = "+356 22915000",
            LivingCountry = location,
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
        var userRepositoryResponse = new Response<UserEntity>(true, data : new UserEntity() { Firstname = "Firstname" });
        _countriesClientMock.Setup(c => c.GetCountryAsync(location)).ReturnsAsync(countryResponse);
        _userRepositoryMock.Setup(u => u.GetByEmailOrUserNameAsync(email, It.IsAny<string>())).ReturnsAsync(userRepositoryResponse);
        _userRepositoryMock.Setup(u => u.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(new ResponseWithoutData(true));
        
        // Act
        var response = await authService.RegisterUserAsync(registrationDto);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be("A user with same email or username already exists");
        response.Data.Should().BeNull();

        _countriesClientMock.Verify(u => u.GetCountryAsync(location), Times.Once);
        _userRepositoryMock.Verify(u => u.GetByEmailOrUserNameAsync(email, It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(u => u.CreateAsync(It.IsAny<UserEntity>()), Times.Never);
    }
}

