using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PollySetup.HealthMonitoring;

public class UserHealthCheck : IHealthCheck 
{

  private volatile bool _isReady = false;
  public bool IsReady { 
    get => _isReady;
    set => _isReady = value; 
  }

  public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) 
  {
        Console.WriteLine("Health check invoked"); // Log that the health check is being invoked

    try
    {
        return Task.FromResult(_isReady ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during health check: {ex.Message}"); // Log any exceptions that occur
        return Task.FromResult(HealthCheckResult.Unhealthy());
    }
  }
}
