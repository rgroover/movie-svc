namespace movie_svc.ViewModels.Actors;

using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class ActorDetails
{
    [JsonProperty("adult", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Adult { get; set; }

    [JsonProperty("also_known_as", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> AlsoKnownAs { get; set; }

    [JsonProperty("biography", NullValueHandling = NullValueHandling.Ignore)]
    public string Biography { get; set; }

    [JsonProperty("birthday", NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset? Birthday { get; set; }

    [JsonProperty("deathday")]
    public object Deathday { get; set; }

    [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
    public long? Gender { get; set; }

    [JsonProperty("homepage")]
    public object Homepage { get; set; }

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public long? Id { get; set; }

    [JsonProperty("imdb_id", NullValueHandling = NullValueHandling.Ignore)]
    public string ImdbId { get; set; }

    [JsonProperty("known_for_department", NullValueHandling = NullValueHandling.Ignore)]
    public string KnownForDepartment { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("place_of_birth", NullValueHandling = NullValueHandling.Ignore)]
    public string PlaceOfBirth { get; set; }

    [JsonProperty("popularity", NullValueHandling = NullValueHandling.Ignore)]
    public double? Popularity { get; set; }

    [JsonProperty("profile_path", NullValueHandling = NullValueHandling.Ignore)]
    public string ProfilePath { get; set; }

    [JsonProperty("combined_credits", NullValueHandling = NullValueHandling.Ignore)]
    public CombinedCredits CombinedCredits { get; set; }
}

public partial class CombinedCredits
{
    [JsonProperty("cast", NullValueHandling = NullValueHandling.Ignore)]
    public List<ActorCast> Cast { get; set; }
}

public partial class ActorCast
{
    [JsonProperty("adult", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Adult { get; set; }

    [JsonProperty("backdrop_path")]
    public string BackdropPath { get; set; }

    [JsonProperty("genre_ids", NullValueHandling = NullValueHandling.Ignore)]
    public List<long> GenreIds { get; set; }

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public long? Id { get; set; }

    [JsonProperty("original_language", NullValueHandling = NullValueHandling.Ignore)]
    public string OriginalLanguage { get; set; }

    [JsonProperty("original_title", NullValueHandling = NullValueHandling.Ignore)]
    public string OriginalTitle { get; set; }

    [JsonProperty("overview", NullValueHandling = NullValueHandling.Ignore)]
    public string Overview { get; set; }

    [JsonProperty("popularity", NullValueHandling = NullValueHandling.Ignore)]
    public double? Popularity { get; set; }

    [JsonProperty("poster_path")]
    public string PosterPath { get; set; }

    [JsonProperty("release_date", NullValueHandling = NullValueHandling.Ignore)]
    public string ReleaseDate { get; set; }

    [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
    public string Title { get; set; }

    [JsonProperty("video", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Video { get; set; }

    [JsonProperty("vote_average", NullValueHandling = NullValueHandling.Ignore)]
    public double? VoteAverage { get; set; }

    [JsonProperty("vote_count", NullValueHandling = NullValueHandling.Ignore)]
    public long? VoteCount { get; set; }

    [JsonProperty("character", NullValueHandling = NullValueHandling.Ignore)]
    public string Character { get; set; }

    [JsonProperty("credit_id", NullValueHandling = NullValueHandling.Ignore)]
    public string CreditId { get; set; }

    [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
    public long? Order { get; set; }

    [JsonProperty("media_type", NullValueHandling = NullValueHandling.Ignore)]
    public string MediaType { get; set; }

    [JsonProperty("origin_country", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> OriginCountry { get; set; }

    [JsonProperty("original_name", NullValueHandling = NullValueHandling.Ignore)]
    public string OriginalName { get; set; }

    [JsonProperty("first_air_date", NullValueHandling = NullValueHandling.Ignore)]
    public string FirstAirDate { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("episode_count", NullValueHandling = NullValueHandling.Ignore)]
    public long? EpisodeCount { get; set; }

    [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
    public string Department { get; set; }

    [JsonProperty("job", NullValueHandling = NullValueHandling.Ignore)]
    public string Job { get; set; }
}
    