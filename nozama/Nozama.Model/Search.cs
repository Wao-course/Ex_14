using System.Text.Json.Serialization;

namespace Nozama.Model;


// Search model class
public class Search
{
    public int Id { get; set; }
    public string? Term { get; set; }
    public int ProductId { get; set; } 

    public DateTimeOffset Timestamp { get; set; }
    
}