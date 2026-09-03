using Microsoft.AspNetCore.Mvc;
using movie_svc.Services;
using movie_svc.ViewModels.Common;
using movie_svc.ViewModels.Movies;

namespace movie_svc.Controllers;

[ApiController]
[Route("/api/movies")]
public class MovieController : ControllerBase
{
    private readonly IRestClientService _restClientService;

    public MovieController(IRestClientService restClientService)
    {
        _restClientService = restClientService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(MovieModel), 200)]
    [Route("/api/movie/{externalId}")]
    public async Task<MovieModel> GetByExternalId(int externalId)
    {
        var movieModel = new MovieModel();
        var request = TmdbRequest.Get($"/movie/{externalId}", ("language", "en-US"));
        MovieDetails movieDetails = await _restClientService.GetAsync<MovieDetails>(request);
        var castAndCrewTask = GetCastForMovie(externalId);
        var watchProvidersTask = GetWatchProviders(externalId);
        var videosTask = GetVideos(externalId);
        await Task.WhenAll(castAndCrewTask, watchProvidersTask, videosTask);

        movieModel.MovieDetails = movieDetails;
        movieModel.CastAndCrew = await castAndCrewTask;
        movieModel.ExternalId = externalId;
        movieModel.WatchProviders = await watchProvidersTask;
        movieModel.Videos = await videosTask;
        return movieModel;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/search/movie/{searchText}")]
    public async Task<SearchResultsPagedModel> MovieSearch(string searchText)
    {
        var request = TmdbRequest.Get("/search/movie", ("include_adult", false), ("language", "en-US"), ("page", 1), ("query", searchText));
        SearchResultsPagedModel searchResultsPagedModel = await _restClientService.GetAsync<SearchResultsPagedModel>(request);
        return searchResultsPagedModel;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/movie/trending")]
    public async Task<SearchResultsPagedModel> MovieTrending()
    {
        var request = TmdbRequest.Get("/trending/movie/week", ("language", "en-US"));
        SearchResultsPagedModel searchResultsPagedModel = await _restClientService.GetAsync<SearchResultsPagedModel>(request);
        return searchResultsPagedModel;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/movie/popular")]
    public async Task<SearchResultsPagedModel> MoviePopular()
    {
        var request = TmdbRequest.Get("/movie/popular", ("language", "en-US"), ("page", 1), ("region", "US"));
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
        var request = TmdbRequest.Get("discover/movie", ("include_adult", false), ("include_video", false), ("language", "en-US"), ("page", 1), ("sort_by", "popularity.desc"), ("with_release_type", "2|3"), ("release_date.gte", minDate), ("release_date.lte", maxDate));
        SearchResultsPagedModel searchResultsPagedModel = await _restClientService.GetAsync<SearchResultsPagedModel>(request);
        return searchResultsPagedModel;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/movie/{externalId}/recommendations")]
    public async Task<SearchResultsPagedModel> GetRecommendations(int externalId, [FromQuery] int page = 1)
    {
        var request = TmdbRequest.Get($"/movie/{externalId}/recommendations", ("language", "en-US"), ("page", page));
        SearchResultsPagedModel recommendations = await _restClientService.GetAsync<SearchResultsPagedModel>(request);
        return recommendations;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ReviewResults), 200)]
    [Route("/api/movie/{externalId}/reviews")]
    public async Task<ReviewResults> GetReviews(int externalId, [FromQuery] int page = 1)
    {
        var request = TmdbRequest.Get($"/movie/{externalId}/reviews", ("language", "en-US"), ("page", page));
        return await _restClientService.GetAsync<ReviewResults>(request);
    }
    
    private async Task<CastAndCrewModel> GetCastForMovie(int externalMovieId)
    {
        var request = TmdbRequest.Get($"/movie/{externalMovieId}/credits", ("language", "en-US"));
        CastAndCrewModel castAndCrew = await _restClientService.GetAsync<CastAndCrewModel>(request);
        return castAndCrew;
    }

    private async Task<WatchProviders> GetWatchProviders(int externalId)
    {
        var request = TmdbRequest.Get($"/movie/{externalId}/watch/providers");
        WatchProviders watchProviders = await _restClientService.GetAsync<WatchProviders>(request);
        return watchProviders;
    }
    
    private async Task<VideoResults> GetVideos(int externalId)
    {
        var request = TmdbRequest.Get($"/movie/{externalId}/videos");
        VideoResults videoResults = await _restClientService.GetAsync<VideoResults>(request);
        return videoResults;
    }
}
