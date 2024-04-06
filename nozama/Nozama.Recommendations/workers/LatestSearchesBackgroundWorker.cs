public class LastestSearchesBackgroundWorker : BackgroundService {

  private readonly ProductCatalogService _service;
  public LastestSearchesBackgroundWorker(ProductCatalogService service) {
    _service = service;
  }
  protected override async Task ExecuteAsync(CancellationToken cancellationToken) 
  {
    while(!cancellationToken.IsCancellationRequested) 
    {
      var recommendation = await _service.GetRecommendations();
      Console.WriteLine(recommendation);
      await Task.Delay(1_000, cancellationToken);
    }
  }
}