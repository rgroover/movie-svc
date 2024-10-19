using Microsoft.AspNetCore.Mvc;
using movie.domain;
using MovieService.Models;
using Newtonsoft.Json;
using RestSharp;

namespace movie_svc.Controllers;

[ApiController]
[Route("/api/cast")]
public class ActorController : ControllerBase
{
    private readonly ILogger<ActorController> _logger;
    private readonly RestClient restClient;
    private readonly IConfiguration _config;
    private readonly string apiKey;

    public ActorController(IConfiguration config, ILogger<ActorController> logger)
    {
        _logger = logger;
        var options = new RestClientOptions("https://api.themoviedb.org/3");
        restClient = new RestClient(options);
        _config = config;
        apiKey = _config["MovieApiKey"];  // comes from user secrets
    }

    [HttpGet]
    [ProducesResponseType(typeof(ActorDetails), 200)]
    [Route("/api/actor/{actorId}")]
    public async Task<ActorDetails> GetById(int actorId)
    {
        var request = SetupAPIRequest(actorId);
        var response = await restClient.GetAsync(request);
        ActorDetails actorDetails;

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            actorDetails = JsonConvert.DeserializeObject<ActorDetails>(response.Content);

        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }

        return actorDetails;
    }

    private RestRequest SetupAPIRequest(int actorId)
    {
            var request = new RestRequest($"/person/{actorId}?language=en-US&append_to_response=combined_credits");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            return request;
    }
}