namespace Weather.DataAccess.Repositories;

public class UserRepository : Repository, IUserRepository
{
    public UserRepository(IOptions<DbConfig> dbConfig) : base(dbConfig)
    {
    }

    public async Task<Response<IEnumerable<UserEntity>>> GetAllAsync()
    {
        var sql = @"SELECT * FROM Users";
        return await QueryAsync<UserEntity>(sql);
    }

    public async Task<ResponseWithoutData> CreateAsync(UserEntity user)
    {
        user.Id = Guid.NewGuid();
        var sql = @"
            INSERT INTO Users (Id, Firstname, Lastname, Username, Email, PasswordHash, Address, Birthdate, PhoneNumber, LivingCountry, CitizenCountry)
            VALUES (@Id, @Firstname, @Lastname, @Username, @Email, @PasswordHash, @Address, @Birthdate, @PhoneNumber, @LivingCountry, @CitizenCountry)
        ";
        return await ExecuteAsync(sql, user);
    }

    public async Task<Response<UserEntity>> GetByEmailOrUserNameAsync(string email, string userName)
    {
        var sql = @"
            SELECT Id, Firstname, Lastname, Username, Email, PasswordHash, Address, Birthdate, PhoneNumber, LivingCountry, CitizenCountry FROM Users 
            WHERE Email = @email Or Username = @userName
        ";
        return await QuerySingleOrDefaultAsync<UserEntity>(sql, new { email, userName });
    }
}