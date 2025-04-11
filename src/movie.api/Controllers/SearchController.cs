using Microsoft.AspNetCore.Mvc;
using movie_svc.Services;
using movie_svc.ViewModels.Movies;
using movie_svc.ViewModels.Search;
using RestSharp;

namespace movie_svc.Controllers;

[ApiController]
[Route("/api/search")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;
    private readonly IRestClientService _restClientService;

    public SearchController(IConfiguration config, ILogger<SearchController> logger, IRestClientService restClientService)
    {
        _logger = logger;
        _restClientService = restClientService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(MultiSearchPagedResults), 200)]
    [Route("/api/search/{searchText}")]
    public async Task<MultiSearchPagedResults> MultiSearch(string searchText, [FromQuery] int page = 1)
    {
        var request = new RestRequest($"/search/multi?query={searchText}&include_adult=false&language=en-US&page={page}");
        MultiSearchPagedResults searchResultsPagedModel = await _restClientService.GetAsync<MultiSearchPagedResults>(request);
        return searchResultsPagedModel;
    }
}