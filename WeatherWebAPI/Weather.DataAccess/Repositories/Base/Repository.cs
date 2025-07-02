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

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<T>(sql);
    }

    public async Task ExecuteAsync<T>(string sql, T entity)
    {
        using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, entity);
        return;
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object data)
    {
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<T>(sql, data);
    }
}
