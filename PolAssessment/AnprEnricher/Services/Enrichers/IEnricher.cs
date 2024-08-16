using PolAssessment.AnprEnricher.Models;

namespace PolAssessment.AnprEnricher.Services.Enrichers;

public interface IEnricher
{
    Task<object> Enrich(AnprData data);
}

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