using Newtonsoft.Json;

namespace movie_svc.ViewModels.TVShows.SeasonsAndEpisodes;

public class EpisodeSummary
{
    [JsonProperty("air_date")]
    public string AirDate { get; set; }

    [JsonProperty("episode_number")]
    public int EpisodeNumber { get; set; }
    
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("still_path")]
    public string StillPath { get; set; }
}

public class TvSeason
{
    public string TvShowTitle { get; set; }
    
    [JsonProperty("air_date")]
    public string AirDate { get; set; }

    [JsonProperty("episodes")]
    public List<EpisodeSummary> Episodes { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }

    [JsonProperty("season_number")]
    public int SeasonNumber { get; set; }

    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }
}
