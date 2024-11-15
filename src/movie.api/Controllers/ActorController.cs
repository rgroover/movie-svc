using Microsoft.AspNetCore.Mvc;
using movie_svc.Services;
using movie_svc.ViewModels.Actors;
using Newtonsoft.Json;
using RestSharp;

namespace movie_svc.Controllers;

[ApiController]
[Route("/api/cast")]
public class ActorController : ControllerBase
{
    private readonly ILogger<ActorController> _logger;
    private readonly IRestClientService _restClientService;

    public ActorController( ILogger<ActorController> logger, IRestClientService restClientService)
    {
        _logger = logger;
        _restClientService = restClientService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ActorDetails), 200)]
    [Route("/api/actor/{actorId}")]
    public async Task<ActorDetails> GetById(int actorId)
    {
        var request = new RestRequest($"/person/{actorId}?language=en-US&append_to_response=combined_credits");
        ActorDetails actorDetails = await _restClientService.GetAsync<ActorDetails>(request);
        return actorDetails;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ActorSearchResults), 200)]
    [Route("/api/search/actor/{searchText}")]
    public async Task<ActorSearchResults> ActorSearch(string searchText)
    {
        var request = new RestRequest($"/search/person?query={searchText}&include_adult=false&language=en-US&page=1");
        ActorSearchResults results = await _restClientService.GetAsync<ActorSearchResults>(request);
        return results;
    }
}