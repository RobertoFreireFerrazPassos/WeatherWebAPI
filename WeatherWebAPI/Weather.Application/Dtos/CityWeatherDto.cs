namespace Weather.Application.Dtos;

public class CityWeatherDto
{
    public CoordDto Coord { get; set; }
    public WeatherDto[] Weather { get; set; }
    public string Base { get; set; }
    public MainDto Main { get; set; }
    public int Visibility { get; set; }
    public WindDto Wind { get; set; }
    public CloudsDto Clouds { get; set; }
    public int Dt { get; set; }
    public SysDto Sys { get; set; }
    public int timezone { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Cod { get; set; }
}