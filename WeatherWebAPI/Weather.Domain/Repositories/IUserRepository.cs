namespace Weather.Domain.Repositories;

public interface IUserRepository
{
    Task<Response<IEnumerable<UserEntity>>> GetAllAsync();

    Task<ResponseWithoutData> CreateAsync(UserEntity user);

    Task<Response<UserEntity>> GetByEmailOrUserNameAsync(string email, string userName);
}