using System.Text.Json.Serialization;

namespace Nozama.Model;

public class Product {
  public int ProductId { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public decimal Price { get; set; }
  [JsonIgnore]
  public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
  [JsonIgnore]
  public ICollection<StatsEntry> StatsEntries { get; set; } = new List<StatsEntry>();

}
