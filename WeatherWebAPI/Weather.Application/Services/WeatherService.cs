namespace Weather.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly IUserRepository _userRepository;

    public WeatherService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Response<bool>> GetWeatherAsync(string username)
    {
        var userFromDbResponse = await _userRepository.GetByEmailOrUserNameAsync(string.Empty, username);

        if (!userFromDbResponse.IsSuccessful)
        {
            return new Response<bool>(false, userFromDbResponse.ErrorMessage);
        }

        return new Response<bool>(true, string.Empty, true);
    }
}
