namespace Weather.CrossCutting.IoC;

public class AppConfig
{
    public RedisCacheConfig RedisCacheConfig { get; set; }

    public ApiConfig RestCountriesApi { get; set;}

    public ApiConfig OpenWeather { get; set; }

    public DbConfig WeatherDb { get; set; }
}
