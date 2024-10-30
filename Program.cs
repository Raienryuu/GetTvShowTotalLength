using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.WriteLine("Hello, World!");


var showName = Environment.GetCommandLineArgs()[1];

async Task<int> GetMostRecentShowId(string name)
{
  HttpClient httpClient = new();
  var response = await httpClient.GetAsync($"https://api.tvmaze.com/search/shows?q={name}");
  var shows = await response.Content.ReadFromJsonAsync<List<Show>>();
  if (shows is null || shows.Count == 0)
  {
	return -1;
  }
  DateOnly newestShowPremiere = new(0, 0, 0);
  int newestShowId = -1;
  foreach (var show in shows)
  {
	if (show.Premiered > newestShowPremiere)
	{
	  newestShowId = show.Id;
	  newestShowPremiere = show.Premiered; 
	}
  }
  return newestShowId;
}


public class Show
{
  [JsonPropertyName("id")]
  public int Id { get; set; }
  [JsonPropertyName("premiered")]
  public DateOnly Premiered { get; set; }
}