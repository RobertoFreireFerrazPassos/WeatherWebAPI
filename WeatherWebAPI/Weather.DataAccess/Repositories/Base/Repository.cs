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

    public async Task<Response<IEnumerable<T>>> QueryAsync<T>(string sql)
    {
        try
        {
            using var connection = CreateConnection();
            var result = await connection.QueryAsync<T>(sql);
            return new Response<IEnumerable<T>>(true, data: result);
        }
        catch (SocketException ex)
        {
            return new Response<IEnumerable<T>>(false, "Error connecting to database");
        }
        catch (Exception ex)
        {
            return new Response<IEnumerable<T>>(false, $"Query error for {typeof(T).Name}");
        }
    }

    public async Task<ResponseWithoutData> ExecuteAsync<T>(string sql, T entity)
    {
        try
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, entity);
            return new ResponseWithoutData(true, string.Empty);
        }
        catch (SocketException ex)
        {
            return new ResponseWithoutData(false, "Error connecting to database");
        }
        catch (Exception ex)
        {
            return new ResponseWithoutData(false, $"Execute query error in {typeof(T).Name}");
        }
    }

    public async Task<Response<T>> QuerySingleOrDefaultAsync<T>(string sql, object data)
    {
        try
        {
            using var connection = CreateConnection();
            var result = await connection.QuerySingleOrDefaultAsync<T>(sql, data);
            return new Response<T>(true, data: result);
        }
        catch (SocketException ex)
        {
            return new Response<T>(false, "Error connecting to database");
        }
        catch (Exception ex)
        {
            return new Response<T>(false, $"Query single error for {typeof(T).Name}");
        }
    }
}
