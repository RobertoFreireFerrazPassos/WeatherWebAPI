namespace Weather.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllAsync();

    Task CreateAsync(UserEntity user);

    Task<UserEntity> GetByEmailOrUserNameAsync(string email, string userName);
}