namespace Weather.Application.Dtos;

public class CountryDto
{
    public IddDto Idd { get; set; }

    public double[] Latlng { get; set; }
}
