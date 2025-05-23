using Microsoft.AspNetCore.Mvc;
using movie_svc.Services;
using movie_svc.ViewModels.Common;
using movie_svc.ViewModels.Movies;
using RestSharp;

namespace movie_svc.Controllers;

[ApiController]
[Route("/api/movies")]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;
    private readonly IRestClientService _restClientService;

    public MovieController(IConfiguration config, ILogger<MovieController> logger, IRestClientService restClientService)
    {
        _logger = logger;
        _restClientService = restClientService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(MovieModel), 200)]
    [Route("/api/movie/{externalId}")]
    public async Task<MovieModel> GetByExternalId(int externalId)
    {
        var movieModel = new MovieModel();
        var request = new RestRequest($"/movie/{externalId}?language=en-US");
        MovieDetails movieDetails = await _restClientService.GetAsync<MovieDetails>(request);
        movieModel.MovieDetails = movieDetails;
        
        var castAndCrew = await GetCastForMovie(externalId);
        var watchProviders = await GetWatchProviders(externalId);
        var videos = await GetVideos(externalId);
        movieModel.CastAndCrew = castAndCrew;
        movieModel.ExternalId = externalId;
        movieModel.WatchProviders = watchProviders; 
        movieModel.Videos = videos;
        return movieModel;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/search/movie/{searchText}")]
    public async Task<SearchResultsPagedModel> MovieSearch(string searchText)
    {
        var request = new RestRequest($"/search/movie?include_adult=false&language=en-US&page=1&query={searchText}");
        SearchResultsPagedModel searchResultsPagedModel = await _restClientService.GetAsync<SearchResultsPagedModel>(request);
        return searchResultsPagedModel;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/movie/trending")]
    public async Task<SearchResultsPagedModel> MovieTrending()
    {
        var request = new RestRequest($"/trending/movie/week?language=en-US");
        SearchResultsPagedModel searchResultsPagedModel = await _restClientService.GetAsync<SearchResultsPagedModel>(request);
        return searchResultsPagedModel;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/movie/popular")]
    public async Task<SearchResultsPagedModel> MoviePopular()
    {
        var request = new RestRequest($"/movie/popular?language=en-US&page=1&region=US");
        SearchResultsPagedModel searchResultsPagedModel = await _restClientService.GetAsync<SearchResultsPagedModel>(request);
        return searchResultsPagedModel;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/movie/now-playing")]
    public async Task<SearchResultsPagedModel> MovieNowPlaying()
    {
        var minDate = DateTime.UtcNow.AddDays(-120).ToString("yyyy-MM-dd");
        var maxDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var request = new RestRequest($"discover/movie?include_adult=false&include_video=false&language=en-US&page=1&sort_by=popularity.desc&with_release_type=2|3&release_date.gte={minDate}&release_date.lte={maxDate}");
        SearchResultsPagedModel searchResultsPagedModel = await _restClientService.GetAsync<SearchResultsPagedModel>(request);
        return searchResultsPagedModel;
    }
    
    private async Task<CastAndCrewModel> GetCastForMovie(int externalMovieId)
    {
        var request = new RestRequest($"/movie/{externalMovieId}/credits?language=en-US");
        CastAndCrewModel castAndCrew = await _restClientService.GetAsync<CastAndCrewModel>(request);
        return castAndCrew;
    }

    private async Task<WatchProviders> GetWatchProviders(int externalId)
    {
        var request = new RestRequest($"/movie/{externalId}/watch/providers");
        WatchProviders watchProviders = await _restClientService.GetAsync<WatchProviders>(request);
        return watchProviders;
    }
    
    private async Task<VideoResults> GetVideos(int externalId)
    {
        var request = new RestRequest($"/movie/{externalId}/videos");
        VideoResults videoResults = await _restClientService.GetAsync<VideoResults>(request);
        return videoResults;
    }
}
