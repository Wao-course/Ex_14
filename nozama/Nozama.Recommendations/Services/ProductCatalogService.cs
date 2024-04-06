using Nozama.Model;

public class ProductCatalogService
{
  private readonly HttpClient _httpClient;

  public ProductCatalogService(HttpClient httpClient)
  {
    _httpClient = httpClient;
    var uri = Environment.GetEnvironmentVariable("PRODUCT_CATALOG_SERVICE_URI");
    if(uri is not null) {
      _httpClient.BaseAddress = new Uri(uri);
    }
  }

  public async Task<IEnumerable<Recommendation>?> GetRecommendations() => await _httpClient.GetFromJsonAsync<IEnumerable<Recommendation>?>("/recommendation");
  public async Task<IEnumerable<StatsEntry>?> GetStats() => await _httpClient.GetFromJsonAsync<IEnumerable<StatsEntry>?>("/stats");
}