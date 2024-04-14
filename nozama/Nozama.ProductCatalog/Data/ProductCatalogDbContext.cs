using Microsoft.EntityFrameworkCore;
using Nozama.Model;

namespace Nozama.ProductCatalog.Data;

public class ProductCatalogDbContext : DbContext
{
  public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options)
  {

  }

  public DbSet<Product> Products => Set<Product>();
  public DbSet<Recommendation> Recommendations => Set<Recommendation>();
  public DbSet<StatsEntry> Stats => Set<StatsEntry>();
  public DbSet<Search> Searches => Set<Search>();
  public async Task<IEnumerable<StatsEntry>> GetStatsFromDatabaseAsync()
  {
    return await Stats.ToListAsync(); // Assuming StatsEntry is the entity representing stats in the database
  }


}