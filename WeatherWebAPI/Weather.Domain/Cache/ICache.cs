namespace Weather.Domain.Cache;

public interface ICache
{
    public string Get(string key);

    public void Set(string key, string value);
}
