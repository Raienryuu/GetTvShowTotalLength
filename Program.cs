using GetTvShowTotalLength;
using System.Net.Http.Json;

HttpClient httpClient = new();
string showName = "";

try {
showName = Environment.GetCommandLineArgs()[1];
}
catch (IndexOutOfRangeException){
  Console.WriteLine("Please provide show's name.");
  return;
}



var showId = await GetMostRecentShowId(showName);
if (showId == -1)
  Environment.Exit(10);

var runtimeInMinutes = await GetShowTotalRuntimeMinutes(showId);

Console.WriteLine("Runtime is : " + runtimeInMinutes);












async Task<int> GetMostRecentShowId(string name)
{
  var response = await httpClient.GetAsync($"https://api.tvmaze.com/search/shows?q={name}");
  if (!response.IsSuccessStatusCode)
	return -1;

  var shows = await response.Content.ReadFromJsonAsync<List<Show>>();
  if (shows is null || shows.Count == 0)
  {
	return -1;
  }

  DateOnly newestShowPremiere = new(1000, 1, 1);
  int newestShowId = -1;
  foreach (var show in shows)
  {
	if (show.ShowDetails.Premiered.CompareTo(newestShowPremiere) > 0)
	{
	  newestShowId = show.ShowDetails.Id;
	  newestShowPremiere = show.ShowDetails.Premiered;
	}
  }
  return newestShowId;
}

async Task<int> GetShowTotalRuntimeMinutes(int showId)
{

  var response = await httpClient.GetAsync($"https://api.tvmaze.com/shows/{showId}/episodes");
  if (!response.IsSuccessStatusCode)
	return -1;

  var episodes = await response.Content.ReadFromJsonAsync<List<ShowEpisode>>();
  if (episodes is null || episodes.Count == 0)
  {
	return -1;
  }

  int totalRuntimeMinutes = 0;
  foreach (var episode in episodes)
  {
	if (episode.Runtime is not null)
	  totalRuntimeMinutes += (int)episode.Runtime;
  }
  return totalRuntimeMinutes;
}
