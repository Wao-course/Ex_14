using Polly;
using Polly.Extensions.Http;
using PollySetup.Controllers;
using PollySetup.HealthMonitoring;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
IAsyncPolicy<HttpResponseMessage> fallbackPolicy =
    Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .FallbackAsync(FallbackAction, OnFallbackAsync);


// Add services to the container.
builder.Services.AddHttpClient(
    "PollyWaitAndRetry",
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:5219/mock");
    }).AddTransientHttpErrorPolicy(
        builder => builder.WaitAndRetryAsync(
            3,
            retryCount => TimeSpan.FromMilliseconds(500),
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                if (outcome?.Result != null)
                {
                    Console.WriteLine($"onRetry {outcome.Result.StatusCode} {outcome.Result.ReasonPhrase} {timespan} {retryAttempt}");
                }
                else
                {
                    Console.WriteLine($"onRetry No response received, retry attempt {retryAttempt}");
                }
            }
        )
    );
    
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




// Add services to the container.
builder.Services.AddSingleton<UserHealthCheck>();
builder.Services.AddSingleton<UserController>();

builder.Services.AddHealthChecks()
    .AddCheck<UserHealthCheck>("user_health_check");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/hc-users");

app.Run();

Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
{
    Console.WriteLine("About to call the fallback action. This is a good place to do some logging");
    return Task.CompletedTask;
}

Task<HttpResponseMessage> FallbackAction(DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context, CancellationToken cancellationToken)
{
    Console.WriteLine("Fallback action is executing");

    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(responseToFailedRequest.Result.StatusCode)
    {
        Content = new StringContent($"The fallback executed, the original error was {responseToFailedRequest.Result.ReasonPhrase}")
    };
    return Task.FromResult(httpResponseMessage);
}
