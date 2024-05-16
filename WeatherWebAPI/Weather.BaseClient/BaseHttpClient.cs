namespace Weather.BaseClient;

public class BaseHttpClient
{
    private readonly HttpClient _httpClient;

    public BaseHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Response<T>> GetAsync<T>(string path)
    {
        try
        {
            var url = _httpClient.BaseAddress + path;

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result is not null)
                {
                    return new Response<T>(true, data: result);
                }
            }

            return new Response<T>(false, "Error during request");
        }
        catch (Exception ex)
        {
            return new Response<T>(false, "Exception during request");
        }
    }
}
