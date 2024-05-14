namespace Weather.Domain.Repositories;

public interface IUserRepository
{
    Task<Response<IEnumerable<User>>> GetAllAsync();

    Task<ResponseWithoutData> CreateAsync(User user);

    Task<Response<User>> GetByEmailOrUserNameAsync(string email, string userName);
}