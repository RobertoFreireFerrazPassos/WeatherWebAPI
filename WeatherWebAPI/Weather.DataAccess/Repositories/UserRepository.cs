namespace Weather.DataAccess.Repositories;

public class UserRepository : Repository, IUserRepository
{
    public UserRepository(IOptions<DbConfig> dbConfig) : base(dbConfig)
    {
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = @"SELECT * FROM Users";
        return await connection.QueryAsync<User>(sql);
    }

    public async Task CreateAsync(User user)
    {
        user.Id = Guid.NewGuid();
        using var connection = CreateConnection();
        var sql = @"
            INSERT INTO Users (Id, Firstname, Lastname, Username, Email, Password, Address, Birthdate, PhoneNumber, LivingCountry, CitizenCountry)
            VALUES (@Id, @Firstname, @Lastname, @Username, @Email, @Password, @Address, @Birthdate, @PhoneNumber, @LivingCountry, @CitizenCountry)
        ";
        await connection.ExecuteAsync(sql, user);
    }

    public async Task<User> GetByEmailOrUserNameAsync(string email, string userName)
    {
        using var connection = CreateConnection();
        var sql = @"
            SELECT Id, Firstname, Lastname, Username, Email, Password, Address, Birthdate, PhoneNumber, LivingCountry, CitizenCountry FROM Users 
            WHERE Email = @email Or Username = @userName
        ";
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email, userName });
    }
}