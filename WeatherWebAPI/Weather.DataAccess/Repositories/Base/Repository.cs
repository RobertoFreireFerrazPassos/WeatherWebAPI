namespace Weather.DataAccess.Repositories.Base;

public class Repository
{
    private readonly IOptions<DbConfig> _dbConfig;

    public Repository(IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_dbConfig.Value.ConnectionString);
    }
}
