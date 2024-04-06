using Microsoft.EntityFrameworkCore;
using Nozama.Model;

public class RecommendationsDbContext : DbContext
{
  public RecommendationsDbContext(DbContextOptions<RecommendationsDbContext> options) : base(options)
  {

  }
  public DbSet<Product> Products => Set<Product>();

  public DbSet<Recommendation> Recommendations => Set<Recommendation>();
  // Inside RecommendationsDbContext class
  public DbSet<StatsEntry> Stats { get; set; }
  public DbSet<Search> Searches { get; set; }


}