namespace PolAssessment.AnprEnricher.Services.Enrichers;

public abstract class BaseHttpClientEnricher(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    protected async Task<string> GetDataFromApiAsync(string url)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}