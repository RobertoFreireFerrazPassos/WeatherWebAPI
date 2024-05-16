namespace Weather.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly IUserRepository _userRepository; 
    
    private readonly IWeatherRepository _weatherRepository;

    private readonly IWeatherClient _weatherClient;

    private readonly ICountriesClient _countriesClient;

    private readonly IMapper _mapper;

    public WeatherService(
        IUserRepository userRepository,
        IWeatherRepository weatherRepository,
        IWeatherClient weatherClient,
        ICountriesClient countriesClient, 
        IMapper mapper)
    {
        _userRepository = userRepository;
        _weatherRepository = weatherRepository;
        _weatherClient = weatherClient;
        _countriesClient = countriesClient;
        _mapper = mapper;
    }

    public async Task<Response<UserWeatherResponse>> GetWeatherAsync(string username)
    {
        var userFromDbResponse = await _userRepository.GetByEmailOrUserNameAsync(string.Empty, username);

        if (!userFromDbResponse.IsSuccessful)
        {
            return new Response<UserWeatherResponse>(userFromDbResponse.IsSuccessful, userFromDbResponse.ErrorMessage);
        }

        var location = userFromDbResponse.Data?.GetLocation();

        if (string.IsNullOrWhiteSpace(location))
        {
            return new Response<UserWeatherResponse>(false, "User doesn't have a location");
        }

        var countryResponse = await _countriesClient.GetCountryAsync(location);


        if (!countryResponse.IsSuccessful)
        {
            return new Response<UserWeatherResponse>(countryResponse.IsSuccessful, countryResponse.ErrorMessage);
        }

        var historicalWeatherResponse = await _weatherRepository.GetByCountryCodeAsync(location);

        if (!historicalWeatherResponse.IsSuccessful)
        {
            return new Response<UserWeatherResponse>(historicalWeatherResponse.IsSuccessful, historicalWeatherResponse.ErrorMessage);
        }

        if (countryResponse.Data is null || countryResponse.Data.Count == 0)
        {
            return new Response<UserWeatherResponse>(false, "Country not found");
        }

        var country = countryResponse.Data[0];
        var lat = country.Latlng[0];
        var lng = country.Latlng[1];

        var cityWeatherResponse = await _weatherClient.GetWeatherAsync(lat, lng);

        if (!cityWeatherResponse.IsSuccessful)
        {
            return new Response<UserWeatherResponse>(cityWeatherResponse.IsSuccessful, cityWeatherResponse.ErrorMessage);
        }

        var userWeather = new UserWeatherResponse()
        {
            HistoricalWeather = _mapper.Map<IEnumerable<HistoricWeatherDto>>(historicalWeatherResponse.Data).ToList(),
            CityWeather = cityWeatherResponse.Data
        };

        return new Response<UserWeatherResponse>(true, data : userWeather);
    }
}
