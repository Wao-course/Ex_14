using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nozama.ProductCatalog.Data;

namespace Nozama.ProductCatalog.HealthMonitoring
{
    public class ProductHealthcheck : IHealthCheck
    {
        private readonly ProductCatalogDbContext _dbContext;

        public ProductHealthcheck(ProductCatalogDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Attempt to query the database to check its health
                await _dbContext.Products.FirstOrDefaultAsync();

                // If the query succeeds, return Healthy status
                return HealthCheckResult.Healthy("Product catalog products are healthy");
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
