using movie_svc.ViewModels.Favorites;

namespace movie_svc.Services;

public interface IFavoritesService
{
    Task<IEnumerable<FavoriteModel>> GetFavorites(string userEmail);
    Task<FavoriteModel> AddFavorite(FavoriteModel favorite);
    Task DeleteFavorite(Guid favoriteId, string userEmail);
}