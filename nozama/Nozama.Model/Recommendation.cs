namespace Nozama.Model;

public record Recommendation {
  public int RecommendationId { get; set; }
  public DateTimeOffset Timestamp { get; set; }
  public ICollection<Product> Products { get; set; } = new List<Product>();

}