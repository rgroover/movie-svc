using Microsoft.AspNetCore.Mvc;
using movie_svc.Services;
using movie_svc.ViewModels.Common;
using movie_svc.ViewModels.TVShows;
using movie_svc.ViewModels.TVShows.SeasonsAndEpisodes;

namespace movie_svc.Controllers;

public class TvShowController : Controller
{
    private readonly IRestClientService _restClientService;
    
    public TvShowController(IRestClientService restClientService)
    {
        _restClientService = restClientService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TVShowSearchResults), 200)]
    [Route("/api/search/tvshow/{searchText}")]
    public async Task<TVShowSearchResults> TvShowSearch(string searchText)
    {
        var request = TmdbRequest.Get("/search/tv", ("query", searchText), ("include_adult", false), ("language", "en-US"), ("page", 1));
        TVShowSearchResults results = await _restClientService.GetAsync<TVShowSearchResults>(request);
        return results;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TVShowSearchResults), 200)]
    [Route("/api/tvshow/trending")]
    public async Task<TVShowSearchResults> TvTrending()
    {
        var request = TmdbRequest.Get("/trending/tv/week", ("language", "en-US"));
        TVShowSearchResults results = await _restClientService.GetAsync<TVShowSearchResults>(request);
        return results;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TVShowSearchResults), 200)]
    [Route("/api/tvshow/popular")]
    public async Task<TVShowSearchResults> TvPopular()
    {
        var request = TmdbRequest.Get("/tv/popular", ("language", "en-US"), ("page", 1));
        TVShowSearchResults results = await _restClientService.GetAsync<TVShowSearchResults>(request);
        return results;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TvShowModel), 200)]
    [Route("/api/tvshow/{externalId}")]
    public async Task<TvShowModel> GetByExternalId(int externalId)
    {
        var request = TmdbRequest.Get($"/tv/{externalId}", ("language", "en-US"), ("append_to_response", "aggregate_credits"));
        TvShowModel tvShowModel = await _restClientService.GetAsync<TvShowModel>(request);
        
        tvShowModel.CastAndCrew.Cast =
            tvShowModel.CastAndCrew.Cast
                .Where(x => x.KnownForDepartment == "Acting")
                .Take(50).ToList();

        var watchProvidersTask = GetWatchProviders(externalId);
        var videosTask = GetVideos(externalId);
        await Task.WhenAll(watchProvidersTask, videosTask);
        tvShowModel.WatchProviders = await watchProvidersTask;
        tvShowModel.Videos = await videosTask;
        return tvShowModel;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TvSeason), 200)]
    [Route("/api/tvshow/{seriesId}/seasons/{seasonNumber}")]
    public async Task<TvSeason> GetSeasonForTvShow(int seriesId, int seasonNumber)
    {
        var request = TmdbRequest.Get($"tv/{seriesId}/season/{seasonNumber}");
        TvSeason tvSeason = await _restClientService.GetAsync<TvSeason>(request);
        
        // call the API to get the Tv Show Name
        request = TmdbRequest.Get($"/tv/{seriesId}");
        TvShowModel tvShowModel = await _restClientService.GetAsync<TvShowModel>(request);
        tvSeason.TvShowTitle = tvShowModel.Name;
        
        return tvSeason;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TvEpisode), 200)]
    [Route("/api/tvshow/{seriesId}/seasons/{seasonNumber}/episode/{episodeNumber}")]
    public async Task<TvEpisode> GetEpisodeForTvShowSeason(int seriesId, int seasonNumber, int episodeNumber)
    {
        var request = TmdbRequest.Get($"/tv/{seriesId}/season/{seasonNumber}/episode/{episodeNumber}");
        TvEpisode tvEpisode = await _restClientService.GetAsync<TvEpisode>(request);
        
        // call the API to get the Tv Show Name
        request = TmdbRequest.Get($"/tv/{seriesId}");
        TvShowModel tvShowModel = await _restClientService.GetAsync<TvShowModel>(request);
        tvEpisode.TvShowTitle = tvShowModel.Name;
        
        return tvEpisode;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ReviewResults), 200)]
    [Route("/api/tvshow/{externalId}/reviews")]
    public async Task<ReviewResults> GetReviews(int externalId, [FromQuery] int page = 1)
    {
        var request = TmdbRequest.Get($"/tv/{externalId}/reviews", ("language", "en-US"), ("page", page));
        return await _restClientService.GetAsync<ReviewResults>(request);
    }
    
    private async Task<WatchProviders> GetWatchProviders(int externalId)
    {
        var request = TmdbRequest.Get($"/tv/{externalId}/watch/providers");
        WatchProviders watchProviders = await _restClientService.GetAsync<WatchProviders>(request);
        return watchProviders;
    }
    
    private async Task<VideoResults> GetVideos(int externalId)
    {
        var request = TmdbRequest.Get($"/tv/{externalId}/videos");
        VideoResults videoResults = await _restClientService.GetAsync<VideoResults>(request);
        return videoResults;
    }
}
