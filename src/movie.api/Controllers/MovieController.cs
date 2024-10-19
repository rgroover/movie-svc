using System.Runtime.InteropServices.Marshalling;
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
    private readonly RestClient restClient;
    private readonly IConfiguration _config;
    private readonly string apiKey;

    public MovieController(IConfiguration config, ILogger<MovieController> logger)
    {
        _logger = logger;
        var options = new RestClientOptions("https://api.themoviedb.org/3");
        restClient = new RestClient(options);
        _config = config;
        apiKey = _config["MovieApiKey"];  // comes from user secrets
    }

    [HttpGet]
    [ProducesResponseType(typeof(MovieModel), 200)]
    [Route("/api/movies/{externalId}")]
    public async Task<MovieModel> GetByExternalId(int externalId)
    {
        var request = SetupAPIRequest(externalId);
        var response = await restClient.GetAsync(request);

        var movieModel = new MovieModel();

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            MovieDetails movieDetails = JsonConvert.DeserializeObject<MovieDetails>(response.Content);
            movieModel.MovieDetails = movieDetails;
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }

        var castAnCrew = await GetCastForMovie(externalId);
        movieModel.CastAndCrew = castAnCrew;
        movieModel.ExternalId = externalId;
        return movieModel;
    }

    private RestRequest SetupAPIRequest(int id)
    {
            var request = new RestRequest($"/movie/{id}?language=en-US");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            return request;
    }

    private async Task<CastAndCrewModel> GetCastForMovie(int externalMovieId)
    {
        CastAndCrewModel castAndCrew = null;

        var request = new RestRequest($"/movie/{externalMovieId}/credits?language=en-US");
        request.AddHeader("accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {apiKey}");

        var response = await restClient.GetAsync(request);

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
    [Route("/api/search/{searchText}")]
    public async Task<SearchResultsPagedModel> MovieSearch(string searchText)
    {
        SearchResultsPagedModel searchResultsPagedModel = null;

        var request = new RestRequest($"/search/movie?include_adult=false&language=en-US&page=1&query={searchText}");
        request.AddHeader("accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {apiKey}");

        var response = await restClient.GetAsync(request);

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            searchResultsPagedModel = JsonConvert.DeserializeObject<SearchResultsPagedModel>(response.Content);
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }
        return searchResultsPagedModel;
    }
}
