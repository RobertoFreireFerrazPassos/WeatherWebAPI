namespace Weather.Application.DataContracts.Responses;

public class UserWeatherResponse
{
    public List<HistoricWeather> HistoricalWeather { get; set; }

    public CityWeather CityWeather  { get; set; }
}