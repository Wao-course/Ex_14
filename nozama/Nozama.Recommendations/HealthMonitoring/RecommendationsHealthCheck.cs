using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nozama.Recommendations.Data;

namespace Nozama.Recommendations.HealthMonitoring
{
    public class RecommendationsHealthCheck : IHealthCheck
    {
        private readonly RecommendationsDbContext _dbContext;
        private readonly ProductCatalogService _productCatalogService;

        public RecommendationsHealthCheck(RecommendationsDbContext dbContext, ProductCatalogService productCatalogService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _productCatalogService = productCatalogService ?? throw new ArgumentNullException(nameof(productCatalogService));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Attempt to query the database to check its health
                await _dbContext.Recommendations.FirstOrDefaultAsync();

                // Attempt to get recommendations from the Product Catalog service to check its health
                await _productCatalogService.GetRecommendations();

                // If both queries succeed, return Healthy status
                return HealthCheckResult.Healthy("Recommendations service is healthy");
            }
            catch (DbUpdateException ex)
            {
                // Handle specific database update exceptions
                return HealthCheckResult.Unhealthy("Database update exception occurred", ex);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return HealthCheckResult.Unhealthy("An exception occurred", ex);
            }
        }
    }
}
