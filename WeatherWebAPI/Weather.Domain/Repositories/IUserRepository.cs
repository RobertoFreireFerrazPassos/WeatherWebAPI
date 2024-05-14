namespace Weather.Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();

    Task CreateAsync(User user);

    Task<User> GetByEmailOrUserNameAsync(string email, string userName);
}