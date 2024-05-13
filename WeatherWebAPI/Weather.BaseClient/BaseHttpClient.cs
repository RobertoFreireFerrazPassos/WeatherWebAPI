namespace Weather.BaseClient;

public class BaseHttpClient
{
    private readonly HttpClient _httpClient;

    public BaseHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> GetAsync<T>(string path)
    {
        try
        {
            var url = _httpClient.BaseAddress + path;

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
        catch (Exception ex)
        {
            return default(T);
        }

        return default(T);
    }
}
