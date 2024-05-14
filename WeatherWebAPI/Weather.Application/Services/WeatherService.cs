namespace Weather.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly IUserRepository _userRepository;

    private readonly IWeatherClient _weatherClient;

    private readonly ICountriesClient _countriesClient;

    public WeatherService(IUserRepository userRepository, IWeatherClient weatherClient,ICountriesClient countriesClient)
    {
        _userRepository = userRepository;
        _weatherClient = weatherClient;
        _countriesClient = countriesClient;
    }

    public async Task<Response<CityWeather>> GetWeatherAsync(string username)
    {
        var userFromDbResponse = await _userRepository.GetByEmailOrUserNameAsync(string.Empty, username);

        if (!userFromDbResponse.IsSuccessful)
        {
            return new Response<CityWeather>(false, userFromDbResponse.ErrorMessage);
        }

        var location = userFromDbResponse.Data?.GetLocation();

        if (string.IsNullOrWhiteSpace(location))
        {
            return new Response<CityWeather>(false, "User doesn't have a location");
        }

        var countryResponse = await _countriesClient.GetCountryAsync(location);


        if (!countryResponse.IsSuccessful)
        {
            return new Response<CityWeather>(countryResponse.IsSuccessful, countryResponse.ErrorMessage);
        }

        if (countryResponse.Data is null || countryResponse.Data.Count == 0)
        {
            return new Response<CityWeather>(false, "Country not found");
        }

        var country = countryResponse.Data[0];
        var lat = country.Latlng[0];
        var lng = country.Latlng[1];

        return await _weatherClient.GetWeatherAsync(lat, lng);
    }
}
