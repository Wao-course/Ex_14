using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Polly;

public class ProductCatalogService
{
    private readonly HttpClient _httpClient;

    public ProductCatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var uri = Environment.GetEnvironmentVariable("PRODUCT_CATALOG_SERVICE_URI");
        if (uri is not null)
        {
            _httpClient.BaseAddress = new Uri(uri);
        }
    }

    public async Task<IEnumerable<Recommendation>?> GetRecommendations()
    {
        // Define the retry policy with exponential backoff
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, // Retry 3 times
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        // Execute the HTTP request with retry policy
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Make the HTTP request to retrieve recommendations
            return await _httpClient.GetFromJsonAsync<IEnumerable<Recommendation>?>("/recommendation");
        });
    }

    public async Task<IEnumerable<StatsEntry>?> GetStats()
    {
        // Define the retry policy with exponential backoff
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, // Retry 3 times
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        // Execute the HTTP request with retry policy
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Make the HTTP request to retrieve stats
            return await _httpClient.GetFromJsonAsync<IEnumerable<StatsEntry>?>("/stats");
        });
    }
}
