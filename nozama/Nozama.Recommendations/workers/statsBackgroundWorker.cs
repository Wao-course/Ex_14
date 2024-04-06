using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Nozama.Model;
using Nozama.Recommendations;

public class StatsBackgroundWorker : BackgroundService
{
    private readonly ProductCatalogService _service;
    private readonly ILogger<StatsBackgroundWorker> _logger;
    private readonly IDbContextFactory<RecommendationsDbContext>? _dbContextFactory;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly HttpClient _httpClient;

    public StatsBackgroundWorker(ILogger<StatsBackgroundWorker> logger,
                                  ProductCatalogService service,
                                  IDbContextFactory<RecommendationsDbContext> dbContext,
                                  IServiceScopeFactory scopeFactory,
                                  HttpClient httpClient)
    {
        _logger = logger;
        _service = service;
        _dbContextFactory = dbContext;
        _scopeFactory = scopeFactory;
        _httpClient = httpClient;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Process stats data
                var statsData = await _service.GetStats();
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<RecommendationsDbContext>();

                    if (statsData != null)
                    {
                        //remove all existing stats data
                        dbContext.Stats.RemoveRange(dbContext.Stats);
                            
                        foreach (var statsEntry in statsData)
                        {
                            if (statsEntry != null)
                            {
                                _logger.LogInformation("Processing stats data for term '{Term}'...", statsEntry.Term);

                                // Remove setting the StatsEntryId, which is an identity column
                                statsEntry.StatsEntryId = 0; // Assuming this property exists and is not set automatically

                                dbContext.Stats.Add(statsEntry); // Add StatsEntry to context
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }

                _logger.LogInformation("Stats data processed and saved to database.");

                // Delay for 1600 milliseconds
                await Task.Delay(1600, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing or saving stats data to the database.");
            }
        }
    }

}
