namespace Weather.Domain.Entities;

public class WeatherEntity : Entity
{
    public string CountryCode { get; set; }

    public string Description { get; set; }

    public string Temperature { get; set; }

    public string Humidity { get; set; }

    public DateTime Time { get; set; }
}
