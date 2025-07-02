namespace Weather.Application.Dtos;

public class UserWeatherDto
{
    public List<HistoricWeatherDto> HistoricalWeather { get; set; }

    public CityWeatherDto CityWeather { get; set; }
}
