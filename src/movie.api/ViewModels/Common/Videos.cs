using Newtonsoft.Json;

namespace movie_svc.ViewModels.Common;

public class Video
{
    [JsonProperty("iso_639_1")]
    public string Iso6391 { get; set; }

    [JsonProperty("iso_3166_1")]
    public string Iso31661 { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("site")]
    public string Site { get; set; }

    [JsonProperty("size")]
    public int Size { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("official")]
    public bool Official { get; set; }

    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }
}

public class VideoResults
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("results")]
    public List<Video> Videos { get; set; }
}

