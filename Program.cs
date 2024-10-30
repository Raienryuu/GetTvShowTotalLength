using GetTvShowTotalLength;
using System.Net.Http.Json;

HttpClient httpClient = new();
string showName = "";

try
{
  showName = Environment.GetCommandLineArgs()[1];
}
catch (IndexOutOfRangeException)
{
  Console.WriteLine("Please provide show's name.");
  return;
}

var show = await GetMostRecentShow(showName);
if (show is null)
    Environment.Exit(10);

var runtimeInMinutes = await GetShowTotalRuntimeMinutes(show.ShowDetails.Id);

Console.WriteLine(show.ShowDetails.Name + ";" + runtimeInMinutes);












async Task<Show?> GetMostRecentShow(string name)
{
  var response = await httpClient.GetAsync($"https://api.tvmaze.com/search/shows?q={name}");
  if (!response.IsSuccessStatusCode)
    return null;

  var shows = await response.Content.ReadFromJsonAsync<List<Show>>();
  if (shows is null || shows.Count == 0)
  {
    return null;
  }

  DateOnly newestShowPremiere = new(1000, 1, 1);
  Show newestShow = new() { ShowDetails = new() };
  foreach (var show in shows)
  {

    if (show.ShowDetails.Premiered is not null &&
      show.ShowDetails.Premiered.Value.CompareTo(newestShowPremiere) > 0)
    {
      newestShow = show;
      newestShowPremiere = show.ShowDetails.Premiered.Value;
    }
  }
  return newestShow;
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
