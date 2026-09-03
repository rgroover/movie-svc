namespace movie_svc.ViewModels.Common;

using Newtonsoft.Json;

public record ReviewAuthor(
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("username")] string Username,
    [property: JsonProperty("rating")] decimal? Rating
);

public record Review(
    [property: JsonProperty("id")] string Id,
    [property: JsonProperty("author")] string Author,
    [property: JsonProperty("author_details")] ReviewAuthor AuthorDetails,
    [property: JsonProperty("content")] string Content,
    [property: JsonProperty("created_at")] DateTime CreatedAt,
    [property: JsonProperty("updated_at")] DateTime UpdatedAt,
    [property: JsonProperty("url")] string Url
);

public record ReviewResults(
    [property: JsonProperty("page")] int Page,
    [property: JsonProperty("results")] List<Review> Results,
    [property: JsonProperty("total_pages")] int TotalPages,
    [property: JsonProperty("total_results")] int TotalResults
);
