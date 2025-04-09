using Newtonsoft.Json;

namespace movie_svc.ViewModels.Search;

public class MultiSearchResult
{
    [JsonProperty("backdrop_path")]
    public string BackdropPath { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("original_name")]
    public string OriginalName { get; set; }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }

    [JsonProperty("media_type")]
    public string MediaType { get; set; }

    [JsonProperty("adult")]
    public bool Adult { get; set; }

    [JsonProperty("original_language")]
    public string OriginalLanguage { get; set; }

    [JsonProperty("genre_ids")]
    public List<int> GenreIds { get; set; }

    [JsonProperty("popularity")]
    public decimal Popularity { get; set; }

    [JsonProperty("first_air_date")]
    public string FirstAirDate { get; set; }

    [JsonProperty("vote_average")]
    public decimal VoteAverage { get; set; }

    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }

    [JsonProperty("origin_country")]
    public List<string> OriginCountry { get; set; }

    [JsonProperty("gender")]
    public int? Gender { get; set; }

    [JsonProperty("known_for_department")]
    public string KnownForDepartment { get; set; }

    [JsonProperty("profile_path")]
    public string ProfilePath { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("original_title")]
    public string OriginalTitle { get; set; }

    [JsonProperty("release_date")]
    public string ReleaseDate { get; set; }

    [JsonProperty("video")]
    public bool? Video { get; set; }
}

public class MultiSearchPagedResults
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("results")]
    public List<MultiSearchResult> Results { get; set; }

    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }

    [JsonProperty("total_results")]
    public int TotalResults { get; set; }
}



