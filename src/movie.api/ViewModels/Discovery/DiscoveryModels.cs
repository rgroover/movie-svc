using Newtonsoft.Json;

namespace movie_svc.ViewModels.Discovery;

public class DiscoveryResults
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("results")]
    public List<DiscoveryResult> Results { get; set; } = [];

    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }

    [JsonProperty("total_results")]
    public int TotalResults { get; set; }
}

public class DiscoveryResult
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("poster_path")]
    public string? PosterPath { get; set; }

    [JsonProperty("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonProperty("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonProperty("first_air_date")]
    public string? FirstAirDate { get; set; }

    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }
}

public class WatchProviderList
{
    [JsonProperty("results")]
    public List<WatchProvider> Results { get; set; } = [];
}

public class WatchProvider
{
    [JsonProperty("provider_id")]
    public int ProviderId { get; set; }

    [JsonProperty("provider_name")]
    public string ProviderName { get; set; } = string.Empty;

    [JsonProperty("logo_path")]
    public string? LogoPath { get; set; }
}
