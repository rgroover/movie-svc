using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_svc.DTOs;
using movie_svc.Services;
using movie_svc.ViewModels.Favorites;

namespace movie_svc.Controllers;

[Authorize]
[ApiController]
[Route("/api/favorites")]
public class FavoritesController : ControllerBase
{
    private readonly IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<FavoriteModel>), 200)]
    public async Task<IActionResult> AddFavorite(FavoriteModel favorite)
    {
        var email = GetAuthenticatedUserEmail();
        favorite.UserEmail = email;
        var newFavorite = await _favoritesService.AddFavorite(favorite);
        return Ok(newFavorite);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FavoriteModel>), 200)]
    public async Task<IActionResult> GetFavorites()
    {
        var email = GetAuthenticatedUserEmail();
        var favorites = await _favoritesService.GetFavorites(email);
        return Ok(favorites);
    }
    
    [HttpDelete]
    [ProducesResponseType(typeof(IEnumerable<FavoriteModel>), 200)]
    public async Task<IActionResult> DeleteFavorite(Guid favoriteId)
    {
        var email = GetAuthenticatedUserEmail();
        await _favoritesService.DeleteFavorite(favoriteId, email);
        return Ok();
    }

    private string GetAuthenticatedUserEmail()
    {
        var userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email) ?? 
            HttpContext.User.FindFirstValue("email");

        if (string.IsNullOrEmpty(userEmail))
        {
            throw new UnauthorizedAccessException("User is not authenticated or email is not in the claims");
        }
        
        return userEmail;
    }
}