namespace Weather.BaseClient;

public class BaseHttpClient(HttpClient _httpClient)
{
    public async Task<T> GetAsync<T>(string path)
    {
        try
        {
            var url = _httpClient.BaseAddress + path;

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result;
            }

            throw new Exception("Request failed");
        }
        catch (Exception ex)
        {
            throw new Exception("Exception during request");
        }
    }
}
