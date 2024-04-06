using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

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
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
