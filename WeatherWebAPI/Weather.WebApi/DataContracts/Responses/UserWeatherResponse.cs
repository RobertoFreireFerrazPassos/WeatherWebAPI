namespace Weather.WebApi.DataContracts.Responses;

public class UserWeatherResponse
{
    public List<HistoricWeatherDto> HistoricalWeather { get; set; }

    public CityWeatherDto CityWeather  { get; set; }
}