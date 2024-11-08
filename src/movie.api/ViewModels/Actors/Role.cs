using Newtonsoft.Json;

namespace movie_svc.ViewModels.Actors;

public class Role
{
    [JsonProperty("credit_id")]
    public string CreditId { get; set; }

    [JsonProperty("character")]
    public string Character { get; set; }

    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }
}