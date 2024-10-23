using Microsoft.AspNetCore.Mvc;
using MovieService.Models;
using Newtonsoft.Json;
using RestSharp;

namespace movie_svc.Controllers;

[ApiController]
[Route("/api/cast")]
public class ActorController : ControllerBase
{
    private readonly ILogger<ActorController> _logger;
    private readonly IRestClient _restClient;

    public ActorController( ILogger<ActorController> logger, IRestClient restClient)
    {
        _logger = logger;
        _restClient = restClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ActorDetails), 200)]
    [Route("/api/actor/{actorId}")]
    public async Task<ActorDetails> GetById(int actorId)
    {
        var request = new RestRequest($"/person/{actorId}?language=en-US&append_to_response=combined_credits");
        var response = await _restClient.GetAsync(request);
        ActorDetails actorDetails;

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            actorDetails = JsonConvert.DeserializeObject<ActorDetails>(response.Content);

        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }

        return actorDetails;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ActorSearchResults), 200)]
    [Route("/api/search/actor/{searchText}")]
    public async Task<ActorSearchResults> ActorSearch(string searchText)
    {
        ActorSearchResults results = null;
        var request = new RestRequest($"/search/person?query={searchText}&include_adult=false&language=en-US&page=1");
        var response = await _restClient.GetAsync(request);

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            results = JsonConvert.DeserializeObject<ActorSearchResults>(response.Content);
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }
        return results;
    }
}