namespace Weather.Tests.Application;

public class WeatherServiceTests
{

    private readonly IMapper _mapper;

    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

    private readonly Mock<IWeatherRepository> _weatherRepositoryMock = new Mock<IWeatherRepository>();

    private readonly Mock<IWeatherClient> _weatherClientMock = new Mock<IWeatherClient>();

    private readonly Mock<ICountriesClient> _countriesClientMock = new Mock<ICountriesClient>();

    public WeatherServiceTests()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ConfigurationAppMapping());
        });
        _mapper = mockMapper.CreateMapper();
    }

    [Fact]
    public async Task GetWeatherAsync_Should_Return_Success_Response()
    {
        // Arrange
        var service = new WeatherService(_userRepositoryMock.Object, _weatherRepositoryMock.Object, _weatherClientMock.Object, _countriesClientMock.Object, _mapper);
        var location = "MLT";
        var lat = 10.03;
        var lng = 20.01;
        var username = "jado12345";
        var weatherList = new List<WeatherEntity>()
        {
            new WeatherEntity()
            {
                CountryCode = location,
                Description = "clear sky",
                Temperature = "295.27"
            }
        };
        var expectedHistoricWeather = new HistoricWeatherDto()
        {
            CountryCode = location,
            Description = "clear sky",
            Temperature = "295.27"
        };
        var userResponse = new Response<UserEntity>(true, data : new UserEntity()
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = "userTeste123@example.com",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004, 01, 12),
            PhoneNumber = "+356 22915000",
            LivingCountry = location,
            CitizenCountry = "MLT"
        });
        var cityWeather = new CityWeatherDto()
        {
            Name = "nameValue",
            Cod = 3
        };
        var countryResponse = new Response<List<CountryDto>>(true, data : new List<CountryDto> { 
            new CountryDto { Latlng = new double[] { lat, lng } } 
        });
        var weatherRepositoryResponse = new Response<IEnumerable<WeatherEntity>>(true, data : weatherList);
        var weatherResponse = new Response<CityWeatherDto>(true, data : cityWeather);

        _userRepositoryMock.Setup(x => x.GetByEmailOrUserNameAsync(string.Empty, username))
                          .ReturnsAsync(userResponse);
        _countriesClientMock.Setup(x => x.GetCountryAsync(location))
                           .ReturnsAsync(countryResponse);
        _weatherRepositoryMock.Setup(x => x.GetByCountryCodeAsync(location))
                             .ReturnsAsync(weatherRepositoryResponse);
        _weatherClientMock.Setup(x => x.GetWeatherAsync(lat, lng))
                         .ReturnsAsync(weatherResponse);

        // Act
        var response = await service.GetWeatherAsync(username);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeNullOrEmpty();
        response.Data.Should().NotBeNull();
        response.Data.HistoricalWeather[0].Should().BeEquivalentTo(expectedHistoricWeather);
        response.Data.CityWeather.Should().Be(cityWeather);

        _userRepositoryMock.Verify(repo => repo.GetByEmailOrUserNameAsync(string.Empty, username), Times.Once);
        _countriesClientMock.Verify(client => client.GetCountryAsync(location), Times.Once);
        _weatherRepositoryMock.Verify(repo => repo.GetByCountryCodeAsync(location), Times.Once);
        _weatherClientMock.Verify(client => client.GetWeatherAsync(lat, lng), Times.Once);
    }

    [Fact]
    public async Task When_UserRepositoryResponseFails_GetWeatherAsync_Should_Return_ErrorMessage()
    {
        // Arrange
        var expectedErrorMessage = "Error happened";
        var service = new WeatherService(_userRepositoryMock.Object, _weatherRepositoryMock.Object, _weatherClientMock.Object, _countriesClientMock.Object, _mapper);
        var location = "MLT";
        var lat = 10.03;
        var lng = 20.01;
        var username = "jado12345";
        var weatherList = new List<WeatherEntity>()
        {
            new WeatherEntity()
            {
                CountryCode = location,
                Description = "clear sky",
                Temperature = "295.27"
            }
        };
        var expectedHistoricWeather = new HistoricWeatherDto()
        {
            CountryCode = location,
            Description = "clear sky",
            Temperature = "295.27"
        };
        var userResponse = new Response<UserEntity>(false, expectedErrorMessage);
        var cityWeather = new CityWeatherDto()
        {
            Name = "nameValue",
            Cod = 3
        };
        var countryResponse = new Response<List<CountryDto>>(true, data: new List<CountryDto> {
            new CountryDto { Latlng = new double[] { lat, lng } }
        });
        var weatherRepositoryResponse = new Response<IEnumerable<WeatherEntity>>(true, data: weatherList);
        var weatherResponse = new Response<CityWeatherDto>(true, data: cityWeather);

        _userRepositoryMock.Setup(x => x.GetByEmailOrUserNameAsync(string.Empty, username))
                          .ReturnsAsync(userResponse);
        _countriesClientMock.Setup(x => x.GetCountryAsync(location))
                           .ReturnsAsync(countryResponse);
        _weatherRepositoryMock.Setup(x => x.GetByCountryCodeAsync(location))
                             .ReturnsAsync(weatherRepositoryResponse);
        _weatherClientMock.Setup(x => x.GetWeatherAsync(lat, lng))
                         .ReturnsAsync(weatherResponse);

        // Act
        var response = await service.GetWeatherAsync(username);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Data.Should().BeNull();

        _userRepositoryMock.Verify(repo => repo.GetByEmailOrUserNameAsync(string.Empty, username), Times.Once);
        _countriesClientMock.Verify(client => client.GetCountryAsync(It.IsAny<string>()), Times.Never);
        _weatherRepositoryMock.Verify(repo => repo.GetByCountryCodeAsync(It.IsAny<string>()), Times.Never);
        _weatherClientMock.Verify(client => client.GetWeatherAsync(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task When_CountriesClientResponseFails_GetWeatherAsync_Should_Return_ErrorMessage()
    {
        // Arrange
        var expectedErrorMessage = "Error happened";
        var service = new WeatherService(_userRepositoryMock.Object, _weatherRepositoryMock.Object, _weatherClientMock.Object, _countriesClientMock.Object, _mapper);
        var location = "MLT";
        var lat = 10.03;
        var lng = 20.01;
        var username = "jado12345";
        var weatherList = new List<WeatherEntity>()
        {
            new WeatherEntity()
            {
                CountryCode = location,
                Description = "clear sky",
                Temperature = "295.27"
            }
        };
        var expectedHistoricWeather = new HistoricWeatherDto()
        {
            CountryCode = location,
            Description = "clear sky",
            Temperature = "295.27"
        };
        var userResponse = new Response<UserEntity>(true, data: new UserEntity()
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = "userTeste123@example.com",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004, 01, 12),
            PhoneNumber = "+356 22915000",
            LivingCountry = location,
            CitizenCountry = "MLT"
        });
        var cityWeather = new CityWeatherDto()
        {
            Name = "nameValue",
            Cod = 3
        };
        var countryResponse = new Response<List<CountryDto>>(false, expectedErrorMessage);
        var weatherRepositoryResponse = new Response<IEnumerable<WeatherEntity>>(true, data: weatherList);
        var weatherResponse = new Response<CityWeatherDto>(true, data: cityWeather);

        _userRepositoryMock.Setup(x => x.GetByEmailOrUserNameAsync(string.Empty, username))
                          .ReturnsAsync(userResponse);
        _countriesClientMock.Setup(x => x.GetCountryAsync(location))
                           .ReturnsAsync(countryResponse);
        _weatherRepositoryMock.Setup(x => x.GetByCountryCodeAsync(location))
                             .ReturnsAsync(weatherRepositoryResponse);
        _weatherClientMock.Setup(x => x.GetWeatherAsync(lat, lng))
                         .ReturnsAsync(weatherResponse);

        // Act
        var response = await service.GetWeatherAsync(username);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Data.Should().BeNull();

        _userRepositoryMock.Verify(repo => repo.GetByEmailOrUserNameAsync(string.Empty, username), Times.Once);
        _countriesClientMock.Verify(client => client.GetCountryAsync(location), Times.Once);
        _weatherRepositoryMock.Verify(repo => repo.GetByCountryCodeAsync(It.IsAny<string>()), Times.Never);
        _weatherClientMock.Verify(client => client.GetWeatherAsync(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task When_WeatherRepositoryFails_GetWeatherAsync_Should_Return_ErrorMessage()
    {
        // Arrange
        var expectedErrorMessage = "Error happened";
        var service = new WeatherService(_userRepositoryMock.Object, _weatherRepositoryMock.Object, _weatherClientMock.Object, _countriesClientMock.Object, _mapper);
        var location = "MLT";
        var lat = 10.03;
        var lng = 20.01;
        var username = "jado12345";
        var expectedHistoricWeather = new HistoricWeatherDto()
        {
            CountryCode = location,
            Description = "clear sky",
            Temperature = "295.27"
        };
        var userResponse = new Response<UserEntity>(true, data: new UserEntity()
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = "userTeste123@example.com",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004, 01, 12),
            PhoneNumber = "+356 22915000",
            LivingCountry = location,
            CitizenCountry = "MLT"
        });
        var cityWeather = new CityWeatherDto()
        {
            Name = "nameValue",
            Cod = 3
        };
        var countryResponse = new Response<List<CountryDto>>(true, data: new List<CountryDto> {
            new CountryDto { Latlng = new double[] { lat, lng } }
        });
        var weatherRepositoryResponse = new Response<IEnumerable<WeatherEntity>>(false, expectedErrorMessage);
        var weatherResponse = new Response<CityWeatherDto>(true, data: cityWeather);

        _userRepositoryMock.Setup(x => x.GetByEmailOrUserNameAsync(string.Empty, username))
                          .ReturnsAsync(userResponse);
        _countriesClientMock.Setup(x => x.GetCountryAsync(location))
                           .ReturnsAsync(countryResponse);
        _weatherRepositoryMock.Setup(x => x.GetByCountryCodeAsync(location))
                             .ReturnsAsync(weatherRepositoryResponse);
        _weatherClientMock.Setup(x => x.GetWeatherAsync(lat, lng))
                         .ReturnsAsync(weatherResponse);

        // Act
        var response = await service.GetWeatherAsync(username);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Data.Should().BeNull();

        _userRepositoryMock.Verify(repo => repo.GetByEmailOrUserNameAsync(string.Empty, username), Times.Once);
        _countriesClientMock.Verify(client => client.GetCountryAsync(location), Times.Once);
        _weatherRepositoryMock.Verify(repo => repo.GetByCountryCodeAsync(location), Times.Once);
        _weatherClientMock.Verify(client => client.GetWeatherAsync(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task When_WeatherClientResponseFails_GetWeatherAsync_Should_Return_ErrorMessage()
    {
        // Arrange
        var expectedErrorMessage = "Error happened";
        var service = new WeatherService(_userRepositoryMock.Object, _weatherRepositoryMock.Object, _weatherClientMock.Object, _countriesClientMock.Object, _mapper);
        var location = "MLT";
        var lat = 10.03;
        var lng = 20.01;
        var username = "jado12345";
        var weatherList = new List<WeatherEntity>()
        {
            new WeatherEntity()
            {
                CountryCode = location,
                Description = "clear sky",
                Temperature = "295.27"
            }
        };
        var expectedHistoricWeather = new HistoricWeatherDto()
        {
            CountryCode = location,
            Description = "clear sky",
            Temperature = "295.27"
        };
        var userResponse = new Response<UserEntity>(true, data: new UserEntity()
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = "userTeste123@example.com",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004, 01, 12),
            PhoneNumber = "+356 22915000",
            LivingCountry = location,
            CitizenCountry = "MLT"
        });
        var countryResponse = new Response<List<CountryDto>>(true, data: new List<CountryDto> {
            new CountryDto { Latlng = new double[] { lat, lng } }
        });
        var weatherRepositoryResponse = new Response<IEnumerable<WeatherEntity>>(true, data: weatherList);
        var weatherResponse = new Response<CityWeatherDto>(false, expectedErrorMessage);

        _userRepositoryMock.Setup(x => x.GetByEmailOrUserNameAsync(string.Empty, username))
                          .ReturnsAsync(userResponse);
        _countriesClientMock.Setup(x => x.GetCountryAsync(location))
                           .ReturnsAsync(countryResponse);
        _weatherRepositoryMock.Setup(x => x.GetByCountryCodeAsync(location))
                             .ReturnsAsync(weatherRepositoryResponse);
        _weatherClientMock.Setup(x => x.GetWeatherAsync(lat, lng))
                         .ReturnsAsync(weatherResponse);

        // Act
        var response = await service.GetWeatherAsync(username);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Data.Should().BeNull();

        _userRepositoryMock.Verify(repo => repo.GetByEmailOrUserNameAsync(string.Empty, username), Times.Once);
        _countriesClientMock.Verify(client => client.GetCountryAsync(location), Times.Once);
        _weatherRepositoryMock.Verify(repo => repo.GetByCountryCodeAsync(location), Times.Once);
        _weatherClientMock.Verify(client => client.GetWeatherAsync(lat, lng), Times.Once);
    }

    [Fact]
    public async Task When_MissingLocation_GetWeatherAsync_Should_Return_ErrorMessage()
    {
        // Arrange
        var expectedErrorMessage = "User doesn't have a location";
        var service = new WeatherService(_userRepositoryMock.Object, _weatherRepositoryMock.Object, _weatherClientMock.Object, _countriesClientMock.Object, _mapper);
        var location = string.Empty;
        var lat = 10.03;
        var lng = 20.01;
        var username = "jado12345";
        var weatherList = new List<WeatherEntity>()
        {
            new WeatherEntity()
            {
                CountryCode = location,
                Description = "clear sky",
                Temperature = "295.27"
            }
        };
        var expectedHistoricWeather = new HistoricWeatherDto()
        {
            CountryCode = location,
            Description = "clear sky",
            Temperature = "295.27"
        };
        var userResponse = new Response<UserEntity>(true, data: new UserEntity()
        {
            Firstname = "Jack",
            Lastname = "Doe",
            Email = "userTeste123@example.com",
            Address = "Bastions Valletta VLT 193",
            Birthdate = new DateTime(2004, 01, 12),
            PhoneNumber = "+356 22915000",
            LivingCountry = location,
            CitizenCountry = "MLT"
        });
        var cityWeather = new CityWeatherDto()
        {
            Name = "nameValue",
            Cod = 3
        };
        var countryResponse = new Response<List<CountryDto>>(true, data: new List<CountryDto> {
            new CountryDto { Latlng = new double[] { lat, lng } }
        });
        var weatherRepositoryResponse = new Response<IEnumerable<WeatherEntity>>(true, data: weatherList);
        var weatherResponse = new Response<CityWeatherDto>(true, data: cityWeather);

        _userRepositoryMock.Setup(x => x.GetByEmailOrUserNameAsync(string.Empty, username))
                          .ReturnsAsync(userResponse);
        _countriesClientMock.Setup(x => x.GetCountryAsync(location))
                           .ReturnsAsync(countryResponse);
        _weatherRepositoryMock.Setup(x => x.GetByCountryCodeAsync(location))
                             .ReturnsAsync(weatherRepositoryResponse);
        _weatherClientMock.Setup(x => x.GetWeatherAsync(lat, lng))
                         .ReturnsAsync(weatherResponse);

        // Act
        var response = await service.GetWeatherAsync(username);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Data.Should().BeNull();

        _userRepositoryMock.Verify(repo => repo.GetByEmailOrUserNameAsync(string.Empty, username), Times.Once);
        _countriesClientMock.Verify(client => client.GetCountryAsync(It.IsAny<string>()), Times.Never);
        _weatherRepositoryMock.Verify(repo => repo.GetByCountryCodeAsync(It.IsAny<string>()), Times.Never);
        _weatherClientMock.Verify(client => client.GetWeatherAsync(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
    }
}