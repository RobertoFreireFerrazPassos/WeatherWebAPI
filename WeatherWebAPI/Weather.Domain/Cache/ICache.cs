namespace Weather.Domain.Cache;

public interface ICache
{
    public Task<string> Get(string key);

    public Task Set(string key, string value);
}
