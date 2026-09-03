using Microsoft.AspNetCore.Mvc;
using movie_svc.Services;
using movie_svc.ViewModels.Discovery;

namespace movie_svc.Controllers;

[ApiController]
[Route("/api/discover")]
public class DiscoveryController(IRestClientService restClientService) : ControllerBase
{
    [HttpGet("{mediaType}")]
    [ProducesResponseType(typeof(DiscoveryResults), StatusCodes.Status200OK)]
    public async Task<ActionResult<DiscoveryResults>> Discover(
        string mediaType,
        [FromQuery] string? genreIds,
        [FromQuery] string? providerIds,
        [FromQuery] string region = "US",
        [FromQuery] double? minRating = null,
        [FromQuery] int? year = null,
        [FromQuery] string sortBy = "popularity.desc",
        [FromQuery] int page = 1)
    {
        if (mediaType is not ("movie" or "tv"))
            return BadRequest("mediaType must be 'movie' or 'tv'.");

        var allowedSorts = mediaType == "movie"
            ? new[] { "popularity.desc", "vote_average.desc", "primary_release_date.desc" }
            : new[] { "popularity.desc", "vote_average.desc", "first_air_date.desc" };
        if (!allowedSorts.Contains(sortBy))
            return BadRequest("Unsupported sort order.");

        var parameters = new List<(string Name, object Value)>
        {
            ("include_adult", false),
            ("language", "en-US"),
            ("page", Math.Clamp(page, 1, 500)),
            ("sort_by", sortBy),
            ("watch_region", region.ToUpperInvariant())
        };

        if (!string.IsNullOrWhiteSpace(genreIds)) parameters.Add(("with_genres", genreIds));
        if (!string.IsNullOrWhiteSpace(providerIds))
        {
            parameters.Add(("with_watch_providers", providerIds));
            parameters.Add(("with_watch_monetization_types", "flatrate"));
        }
        if (minRating.HasValue) parameters.Add(("vote_average.gte", Math.Clamp(minRating.Value, 0, 10)));
        if (year.HasValue) parameters.Add((mediaType == "movie" ? "primary_release_year" : "first_air_date_year", year.Value));

        var request = TmdbRequest.Get($"/discover/{mediaType}", parameters.ToArray());
        return await restClientService.GetAsync<DiscoveryResults>(request);
    }

    [HttpGet("providers/{mediaType}")]
    [ProducesResponseType(typeof(WatchProviderList), StatusCodes.Status200OK)]
    public async Task<ActionResult<WatchProviderList>> GetProviders(string mediaType, [FromQuery] string region = "US")
    {
        if (mediaType is not ("movie" or "tv"))
            return BadRequest("mediaType must be 'movie' or 'tv'.");

        var request = TmdbRequest.Get($"/watch/providers/{mediaType}", ("watch_region", region.ToUpperInvariant()), ("language", "en-US"));
        return await restClientService.GetAsync<WatchProviderList>(request);
    }
}
