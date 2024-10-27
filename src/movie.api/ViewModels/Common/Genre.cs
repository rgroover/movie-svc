namespace movie_svc.ViewModels.Common;

using Newtonsoft.Json;

public record Genre(
    [property: JsonProperty("id")] int Id,
    [property: JsonProperty("name")] string Name
);