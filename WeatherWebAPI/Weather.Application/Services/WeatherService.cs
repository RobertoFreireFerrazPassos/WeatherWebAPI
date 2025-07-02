namespace Weather.Application.Services;

public class WeatherService(
        IUserRepository _userRepository,
        IWeatherRepository _weatherRepository,
        IWeatherClient _weatherClient,
        ICountriesClient _countriesClient,
        IMapper _mapper) : IWeatherService
{
    public async Task<UserWeatherDto> GetWeatherAsync(string username)
    {
        var userFromDb = await _userRepository.GetByEmailOrUserNameAsync(string.Empty, username);

        var location = userFromDb?.GetLocation();

        if (string.IsNullOrWhiteSpace(location))
        {
            throw new Exception("User doesn't have a location");
        }

        var countries = await _countriesClient.GetCountryAsync(location);

        var historicalWeather = await _weatherRepository.GetByCountryCodeAsync(location);

        if (countries is null || countries.Count == 0)
        {
            throw new Exception("Country not found");
        }

        var country = countries[0];
        var lat = country.Latlng[0];
        var lng = country.Latlng[1];

        var cityWeather = await _weatherClient.GetWeatherAsync(lat, lng);

        return new UserWeatherDto()
        {
            HistoricalWeather = _mapper.Map<IEnumerable<HistoricWeatherDto>>(historicalWeather).ToList(),
            CityWeather = cityWeather
        };
    }
}
