using System.Text.Json.Serialization;

namespace GetTvShowTotalLength;

public class ShowEpisode
{
  [JsonPropertyName("season")]
  public int? Season { get; set; }
  [JsonPropertyName("number")]
  public int? Number { get; set; }
  [JsonPropertyName("runtime")]
  public int? Runtime { get; set; } = 0;
}