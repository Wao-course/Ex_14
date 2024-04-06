using Microsoft.EntityFrameworkCore;
using Nozama.Recommendations.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<ProductCatalogService>();
builder.Services.AddScoped<ProductLookupService>(); // AddScoped is just one option; use the appropriate lifetime for your scenario
builder.Services.AddHostedService<LastestSearchesBackgroundWorker>();
builder.Services.AddHostedService<StatsBackgroundWorker>();
builder.Services.AddControllers();

builder.Services.AddDbContextFactory<RecommendationsDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING"))
);
var app = builder.Build();

// This approach should not be used in production. See https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying
using(var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<RecommendationsDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log the exception or handle it gracefully
        Console.WriteLine($"Error occurred during database migration: {ex.Message}");
    }
}

app.MapGet("/", () => "Main page of the Recommendations service");
app.MapControllers();

app.Run();
