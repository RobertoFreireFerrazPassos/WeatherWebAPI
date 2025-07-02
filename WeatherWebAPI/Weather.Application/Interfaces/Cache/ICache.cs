namespace Weather.Application.Interfaces.Cache;

public interface ICache
{
    public Task<string> GetAsync(string key);

    public Task SetAsync(string key, string value);
}
