namespace PolAssessment.AnprEnricher.Services.Enrichers;

public abstract class BaseHttpClientEnricher(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    protected async Task<string> GetDataFromApiAsync(string url)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to get data from API. Status code: {response.StatusCode}");
        }

        return await response.Content.ReadAsStringAsync();
    }
}