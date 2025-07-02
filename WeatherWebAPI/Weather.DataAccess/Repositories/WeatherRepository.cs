namespace Weather.DataAccess.Repositories;

public class WeatherRepository : Repository, IWeatherRepository
{
    public WeatherRepository(IOptions<DbConfig> dbConfig) : base(dbConfig)
    {
    }

    public async Task<IEnumerable<WeatherEntity>> GetByCountryCodeAsync(string countryCode)
    {
        var sql = $@"
            SELECT Id, CountryCode, Description, Temperature, Humidity, Time FROM Weather 
            WHERE CountryCode = '{countryCode}'
        ";
        return await QueryAsync<WeatherEntity>(sql);
    }
}