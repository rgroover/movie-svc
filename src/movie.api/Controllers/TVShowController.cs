using Microsoft.AspNetCore.Mvc;
using movie_svc.Services;
using movie_svc.ViewModels.Common;
using movie_svc.ViewModels.TVShows;
using Newtonsoft.Json;
using RestSharp;

namespace movie_svc.Controllers;

public class TvShowController : Controller
{
    private readonly IRestClientService _restClientService;
    
    public TvShowController( ILogger<ActorController> logger, IRestClientService restClientService)
    {
        _restClientService = restClientService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TVShowSearchResults), 200)]
    [Route("/api/search/tvshow/{searchText}")]
    public async Task<TVShowSearchResults> TvShowSearch(string searchText)
    {
        var request = new RestRequest($"/search/tv?query={searchText}&include_adult=false&language=en-US&page=1");
        TVShowSearchResults results = await _restClientService.GetAsync<TVShowSearchResults>(request);
        return results;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TVShowSearchResults), 200)]
    [Route("/api/tvshow/trending")]
    public async Task<TVShowSearchResults> TvTrending()
    {
        var request = new RestRequest($"/trending/tv/week?language=en-US");
        TVShowSearchResults results = await _restClientService.GetAsync<TVShowSearchResults>(request);
        return results;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TVShowSearchResults), 200)]
    [Route("/api/tvshow/popular")]
    public async Task<TVShowSearchResults> TvPopular()
    {
        var request = new RestRequest($"/tv/popular?language=en-US&page=1");
        TVShowSearchResults results = await _restClientService.GetAsync<TVShowSearchResults>(request);
        return results;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TvShowModel), 200)]
    [Route("/api/tvshow/{externalId}")]
    public async Task<TvShowModel> GetByExternalId(int externalId)
    {
        var request = new RestRequest($"/tv/{externalId}?language=en-US&append_to_response=aggregate_credits");
        TvShowModel tvShowModel = await _restClientService.GetAsync<TvShowModel>(request);
        
        tvShowModel.CastAndCrew.Cast =
            tvShowModel.CastAndCrew.Cast
                .Where(x => x.KnownForDepartment == "Acting")
                .Take(50).ToList();

        var watchProviders = await GetWatchProviders(externalId);
        var videos = await GetVideos(externalId);
        tvShowModel.WatchProviders = watchProviders;
        tvShowModel.Videos = videos;
        return tvShowModel;
    }
    
    private async Task<WatchProviders> GetWatchProviders(int externalId)
    {
        var request = new RestRequest($"/tv/{externalId}/watch/providers");
        WatchProviders watchProviders = await _restClientService.GetAsync<WatchProviders>(request);
        return watchProviders;
    }
    
    private async Task<VideoResults> GetVideos(int externalId)
    {
        var request = new RestRequest($"/tv/{externalId}/videos");
        VideoResults videoResults = await _restClientService.GetAsync<VideoResults>(request);
        return videoResults;
    }
}