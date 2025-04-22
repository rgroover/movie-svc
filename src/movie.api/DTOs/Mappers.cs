using movie_svc.ViewModels.Favorites;

namespace movie_svc.DTOs;

public static class Mappers
{
    public static FavoriteDto MapFavoriteVMtoDto(FavoriteModel favorite)
    {
        var favoriteDto = new FavoriteDto()
        {
            UserEmail = favorite.UserEmail,
            Id = favorite.Id,
            MediaTitle =  favorite.MediaTitle,
            MediaId =  favorite.MediaId,
            MediaImageUrl =  favorite.MediaImageUrl,
            MediaType =  favorite.MediaType,
            CreatedAt = favorite.CreatedAt
        };
        return favoriteDto;
    }
    
    public static FavoriteModel MapFavoriteDtoToVM(FavoriteDto favorite)
    {
        var favoriteModel = new FavoriteModel()
        {
            UserEmail = favorite.UserEmail,
            Id = favorite.Id,
            MediaTitle =  favorite.MediaTitle,
            MediaId =  favorite.MediaId,
            MediaImageUrl =  favorite.MediaImageUrl,
            MediaType =  favorite.MediaType,
            CreatedAt = favorite.CreatedAt
        };
        return favoriteModel;
    }
}