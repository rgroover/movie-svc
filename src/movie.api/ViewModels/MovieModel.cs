using Newtonsoft.Json;

namespace movie.domain
{
    public class MovieModel
    {
        public int ExternalId { get; set; }
        public MovieDetails MovieDetails { get; set; }
        public CastAndCrewModel CastAndCrew { get; set; }
    }
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public record Genre(
        [property: JsonProperty("id")] int Id,
        [property: JsonProperty("name")] string Name
    );

    public record ProductionCompany(
        [property: JsonProperty("id")] int Id,
        [property: JsonProperty("logo_path")] string LogoPath,
        [property: JsonProperty("name")] string Name,
        [property: JsonProperty("origin_country")] string OriginCountry
    );

    public record ProductionCountry(
        [property: JsonProperty("iso_3166_1")] string Iso31661,
        [property: JsonProperty("name")] string Name
    );

    public record MovieDetails(
        [property: JsonProperty("adult")] bool Adult,
        [property: JsonProperty("backdrop_path")] string BackdropPath,
        [property: JsonProperty("belongs_to_collection")] object BelongsToCollection,
        [property: JsonProperty("budget")] int Budget,
        [property: JsonProperty("genres")] IReadOnlyList<Genre> Genres,
        [property: JsonProperty("homepage")] string Homepage,
        [property: JsonProperty("id")] int Id,
        [property: JsonProperty("imdb_id")] string ImdbId,
        [property: JsonProperty("origin_country")] IReadOnlyList<string> OriginCountry,
        [property: JsonProperty("original_language")] string OriginalLanguage,
        [property: JsonProperty("original_title")] string OriginalTitle,
        [property: JsonProperty("overview")] string Overview,
        [property: JsonProperty("popularity")] double Popularity,
        [property: JsonProperty("poster_path")] string PosterPath,
        [property: JsonProperty("production_companies")] IReadOnlyList<ProductionCompany> ProductionCompanies,
        [property: JsonProperty("production_countries")] IReadOnlyList<ProductionCountry> ProductionCountries,
        [property: JsonProperty("release_date")] string ReleaseDate,
        [property: JsonProperty("revenue")] int Revenue,
        [property: JsonProperty("runtime")] int Runtime,
        [property: JsonProperty("spoken_languages")] IReadOnlyList<SpokenLanguage> SpokenLanguages,
        [property: JsonProperty("status")] string Status,
        [property: JsonProperty("tagline")] string Tagline,
        [property: JsonProperty("title")] string Title,
        [property: JsonProperty("video")] bool Video,
        [property: JsonProperty("vote_average")] double VoteAverage,
        [property: JsonProperty("vote_count")] int VoteCount
    );

    public record SpokenLanguage(
        [property: JsonProperty("english_name")] string EnglishName,
        [property: JsonProperty("iso_639_1")] string Iso6391,
        [property: JsonProperty("name")] string Name
    );


}