using Microsoft.EntityFrameworkCore;
using Nozama.ProductCatalog.Data;
using System.Data.Common;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<ProductCatalogDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTION_STRING"))
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// This approach should not be used in production. See https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying
using(var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
