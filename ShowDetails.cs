using System.Text.Json.Serialization;

namespace GetTvShowTotalLength;

public class Show
{
  [JsonPropertyName("show")]
  public required ShowDetails ShowDetails { get; set; }
}

public class ShowDetails
{

  [JsonPropertyName("id")]
  public int Id { get; set; }
  [JsonPropertyName("premiered")]
  public DateOnly Premiered { get; set; }
}