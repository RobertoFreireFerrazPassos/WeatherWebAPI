namespace Weather.DataAccess.Repositories;

public class UserRepository : Repository, IUserRepository
{
    public UserRepository(IOptions<DbConfig> dbConfig) : base(dbConfig)
    {
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        var sql = @"SELECT * FROM UserRegistration";
        return await QueryAsync<UserEntity>(sql);
    }

    public async Task CreateAsync(UserEntity user)
    {
        user.Id = Guid.NewGuid();
        var sql = @"
            INSERT INTO UserRegistration (Id, Firstname, Lastname, Username, Email, PasswordHash, Address, Birthdate, PhoneNumber, LivingCountry, CitizenCountry)
            VALUES (@Id, @Firstname, @Lastname, @Username, @Email, @PasswordHash, @Address, @Birthdate, @PhoneNumber, @LivingCountry, @CitizenCountry)
        ";
        await ExecuteAsync(sql, user);
    }

    public async Task<UserEntity> GetByEmailOrUserNameAsync(string email, string userName)
    {
        var sql = @"
            SELECT Id, Firstname, Lastname, Username, Email, PasswordHash, Address, Birthdate, PhoneNumber, LivingCountry, CitizenCountry FROM UserRegistration 
            WHERE Email = @email Or Username = @userName
        ";
        return await QuerySingleOrDefaultAsync<UserEntity>(sql, new { email, userName });
    }
}