using System.Text.Json.Serialization;
using movie_svc.Utils.Converters;

namespace movie_svc.ViewModels.Favorites;

public class FavoriteModel
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; }
    public string MediaId { get; set; }
    public string MediaTitle { get; set; }
    public string MediaImageUrl { get; set; }
    public string MediaType { get; set; }
    public DateTime CreatedAt { get; set; }
}