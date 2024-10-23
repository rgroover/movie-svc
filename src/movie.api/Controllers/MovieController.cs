using Microsoft.AspNetCore.Mvc;
using movie.domain;
using Newtonsoft.Json;
using RestSharp;

namespace movie_svc.Controllers;

[ApiController]
[Route("/api/movies")]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;
    private readonly IRestClient _restClient;

    public MovieController(IConfiguration config, ILogger<MovieController> logger, IRestClient restClient)
    {
        _logger = logger;
        _restClient = restClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(MovieModel), 200)]
    [Route("/api/movies/{externalId}")]
    public async Task<MovieModel> GetByExternalId(int externalId)
    {
        var request = new RestRequest($"/movie/{externalId}?language=en-US");
        var response = await _restClient.GetAsync(request);

        var movieModel = new MovieModel();

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            MovieDetails movieDetails = JsonConvert.DeserializeObject<MovieDetails>(response.Content);
            movieModel.MovieDetails = movieDetails;
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }

        var castAndCrew = await GetCastForMovie(externalId);
        movieModel.CastAndCrew = castAndCrew;
        movieModel.ExternalId = externalId;
        return movieModel;
    }

    private async Task<CastAndCrewModel> GetCastForMovie(int externalMovieId)
    {
        CastAndCrewModel castAndCrew = null;

        var request = new RestRequest($"/movie/{externalMovieId}/credits?language=en-US");
        var response = await _restClient.GetAsync(request);

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            castAndCrew = JsonConvert.DeserializeObject<CastAndCrewModel>(response.Content);
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }
        return castAndCrew;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(SearchResultsPagedModel), 200)]
    [Route("/api/search/movie/{searchText}")]
    public async Task<SearchResultsPagedModel> MovieSearch(string searchText)
    {
        SearchResultsPagedModel searchResultsPagedModel = null;

        var request = new RestRequest($"/search/movie?include_adult=false&language=en-US&page=1&query={searchText}");
        var response = await _restClient.GetAsync(request);

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            searchResultsPagedModel = JsonConvert.DeserializeObject<SearchResultsPagedModel>(response.Content);
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }
        return searchResultsPagedModel;
    }
}
