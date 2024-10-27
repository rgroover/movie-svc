namespace movie_svc.ViewModels.TVShows;

using Newtonsoft.Json;

public class TVShowSearchResultRecord
{
    [JsonProperty("adult")]
    public bool Adult { get; set; }

    [JsonProperty("backdrop_path")]
    public string BackdropPath { get; set; }

    [JsonProperty("genre_ids")]
    public List<int> GenreIds { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("origin_country")]
    public List<string> OriginCountry { get; set; }

    [JsonProperty("original_language")]
    public string OriginalLanguage { get; set; }

    [JsonProperty("original_name")]
    public string OriginalName { get; set; }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    [JsonProperty("popularity")]
    public double Popularity { get; set; }

    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }

    [JsonProperty("first_air_date")]
    public string FirstAirDate { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }

    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }
}

public class TVShowSearchResults
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("results")]
    public List<TVShowSearchResultRecord> Results { get; set; }

    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }

    [JsonProperty("total_results")]
    public int TotalResults { get; set; }
}