# Exercise
We're going to continue development on the Nozama shopping API. (Part of a solution to last week - not updated to .NET 8 https://gitlab.au.dk/swwao/nozama/-/tree/lesson-12?ref_type=heads)

But before we get back to Nozama, we'll explore Polly and health monitoring in an isolated project:

### Setup Polly
1. Create a new .NET project using the ASP.NET Core WebAPI template[^5]
  > created the folder PollySetup and ran the following command:
  > ```bash
  > dotnet new webapi -n <name>
  > ```

2. Add Polly to your project[^4]
3. Add new controller to the project named `MockController.cs` and copy the following code (or grap it @ `../TransientFaultHandling/Controllers/MockController.cs`):
    ```cs
    using Microsoft.AspNetCore.Mvc;

    namespace TransientFaultHandling.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class MockController: ControllerBase {

      public enum EndpointState {
        Fail,
        Ok,
        Slow,
      }
      public MockController() {

      }
      [Route("success")]
      [HttpGet]
      public Task<StatusCodeResult> OnGetSuccess() {
        return Task.FromResult(new StatusCodeResult(StatusCodes.Status200OK));
      }

      [HttpGet]
      public Task<StatusCodeResult> OnGet() {
        var rand = (EndpointState)new Random().Next(0,3);
        var result = StatusCodes.Status418ImATeapot;
        switch (rand) {
          case EndpointState.Fail:
            result = StatusCodes.Status500InternalServerError;
            break;
          case EndpointState.Ok:
            result = StatusCodes.Status200OK;
            break;
          case EndpointState.Slow:
            result = StatusCodes.Status408RequestTimeout;
          break;
        }
        return Task.FromResult(new StatusCodeResult(result));
      }
    }
    ```
4. Setup a Retry with exponential backoff with jitter[^1] targeting the `GET /` route in `MockController.cs`. Make sure to use `IHttpClientFactory`[^9] to create a named client for the endpoint
5. Create a controller named `UserController`, inject `IHttpClientFactory` and implement an endpoint calling `GET /` on the instance 
  > Uncomment the code inside [PollyUserController.cs](./TransientFaultHandling/Controllers/UserController.cs) then change the name of the file to `UserController.cs`
  
6. Test it out with Postman[^6]

## Exercise 09-2
### Setup health monitoring
Next up, we're going to setup endpoint monitoring for our controllers.

1. Copy the following code into a file called `UserHealthCheck.cs` (or grap it @ `../HealthMonitoring/UserHealthCheck.cs`):

    ```cs
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    namespace HealthMonitoring.HealthChecks;

    public class UserHealthCheck : IHealthCheck 
    {

      private volatile bool _isReady = false;
      public bool IsReady { 
        get => _isReady;
        set => _isReady = value; 
      }

      public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) 
      {
        return Task.FromResult(_isReady ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
      }
    }
    ```

    Register `UserHealthCheck` as a service in `Program.cs` (so we can inject it in our controllers later)
      
      ```csharp
      // Add services to the container.
      builder.Services.AddSingleton<UserHealthCheck>();
      builder.Services.AddSingleton<UserController>();
      builder.Services.AddHealthChecks().AddCheck<UserHealthCheck>("UserHealthCheck");
      ```

2. In `UserController`, add an endpoint @ `GET /ready` that toggles the `IsReady` property in `UserHealthCheck`
3. Configure the `UserHealthCheck` health check and route it to `/hc-users`. There is a great guide[^7] @ Microsoft Docs
4. Test it out in a browser
5. Optional: Come back after adding a circuit breaker and set up the UI by following the guide[^8] @ GitHub (remember to read the whole section carefully, there are some hidden "gems")

### Add a circuit breaker
1. Add a circuit breaker that opens after three (3) failures, stays open for 10 seconds before switching to a half-open state[^2][^3] (see `../TransientFaultHandling/Program.cs` for inspiration)
```csharp	
builder.Services.AddHttpClient(
    "PollyCircuitBreaker",
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:5219/mock");
    }).AddTransientHttpErrorPolicy(
        builder => builder.CircuitBreakerAsync(
            3,
    TimeSpan.FromSeconds(10),
    onBreak: (outcome, timespan, context) => Console.WriteLine($"Circuit is open due to {outcome.Exception}. Waiting for {timespan} before attempting to reset."),
    onHalfOpen: () => Console.WriteLine("Circuit is half-open. Trying a test request."),
    onReset: (context) => Console.WriteLine("Circuit is closed again.")
)).AddPolicyHandler(fallbackPolicy);

```



## Nozama

### Add robustness to Recommendation service
Add Polly and setup an exponential backoff retry in `Nozama.Recommendations/ProductCatalogService.cs`.

### Setup monitoring endpoints
Add monitoring endpoints to `Nozama.ProductCatalog` and `Nozama.Recommendations`.

### Add caching capability to `/productcatalog`
- We're going to add some simple caching to the `ProductCatalogService` (see p. 152 in Microservices in .NET, Second edition)


### Visualize the setup
```mermaid
flowchart TD
    subgraph "Nozama Microservices"
        subgraph "Nozama.Recommendations"
            RecommendationsService["Recommendations Service"] --> RecommendationsDB["Recommendations Database"]
        end
        subgraph "Nozama.ProductCatalog"
            ProductCatalogService["Product Catalog Service"] --> ProductCatalogDB["Product Catalog Database"]
        end
    end
    subgraph "Client Application"
        Browser["Web Browser"]
    end

    Browser --> RecommendationsService
    Browser --> ProductCatalogService

    RecommendationsService -->|Database Query| RecommendationsDB
    ProductCatalogService -->|Database Query| ProductCatalogDB


```

[^1]: https://github.com/App-vNext/Polly/wiki/Retry-with-jitter
[^2]: https://docs.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker
[^3]: https://github.com/App-vNext/Polly#circuit-breaker
[^4]: https://www.nuget.org/packages/Microsoft.Extensions.Http.Polly
[^5]: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new
[^6]: https://www.postman.com/
[^7]: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks
[^8]: https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks#HealthCheckUI
[^9]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
