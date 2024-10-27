namespace movie_svc.ViewModels.Common;

using Newtonsoft.Json;
public record SpokenLanguage(
    [property: JsonProperty("english_name")] string EnglishName,
    [property: JsonProperty("iso_639_1")] string Iso6391,
    [property: JsonProperty("name")] string Name
);

public record ProductionCountry(
    [property: JsonProperty("iso_3166_1")] string Iso31661,
    [property: JsonProperty("name")] string Name
);

public record ProductionCompany(
    [property: JsonProperty("id")] int Id,
    [property: JsonProperty("logo_path")] string LogoPath,
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("origin_country")] string OriginCountry
);