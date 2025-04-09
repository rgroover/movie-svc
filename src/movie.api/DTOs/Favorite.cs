using System.Text.Json.Serialization;
using movie_svc.Utils.Converters;

namespace movie_svc.DTOs;

public class FavoriteDto
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(GuidToStringConverter))]
    public Guid Id { get; set; }
    
    [JsonPropertyName("user_email")]
    public string UserEmail { get; set; }
    
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
    
    [JsonPropertyName("media_title")]
    public string MediaTitle { get; set; }
    
    [JsonPropertyName("media_image_url")]
    public string MediaImageUrl { get; set; }
    
    [JsonPropertyName("media_type")]
    public string MediaType { get; set; }
    
    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(DateTimeToIsoConverter))] 
    public DateTime CreatedAt { get; set; }
}