using Microsoft.AspNetCore.Mvc;
using movie_svc.ViewModels.TVShows;
using Newtonsoft.Json;
using RestSharp;

namespace movie_svc.Controllers;

public class TvShowController : Controller
{
    private readonly IRestClient _restClient;
    
    public TvShowController( ILogger<ActorController> logger, IRestClient restClient)
    {
        _restClient = restClient;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TVShowSearchResults), 200)]
    [Route("/api/search/tvshow/{searchText}")]
    public async Task<TVShowSearchResults> TvShowSearch(string searchText)
    {
        TVShowSearchResults results = null;
        var request = new RestRequest($"/search/tv?query={searchText}&include_adult=false&language=en-US&page=1");
        var response = await _restClient.GetAsync(request);

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            results = JsonConvert.DeserializeObject<TVShowSearchResults>(response.Content);
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }
        return results;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TvShowModel), 200)]
    [Route("/api/tvshow/{externalId}")]
    public async Task<TvShowModel> GetByExternalId(int externalId)
    {
        var request = new RestRequest($"/tv/{externalId}?language=en-US&append_to_response=aggregate_credits");
        var response = await _restClient.GetAsync(request);

        var tvShowModel = new TvShowModel();

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            tvShowModel = JsonConvert.DeserializeObject<TvShowModel>(response.Content);
            tvShowModel.CastAndCrew.Cast =
                tvShowModel.CastAndCrew.Cast
                    .Where(x => x.KnownForDepartment == "Acting")
                    .Take(50).ToList();
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }
        var watchProviders = await GetWatchProviders(externalId);
        tvShowModel.WatchProviders = watchProviders;
        return tvShowModel;
    }
    
    private async Task<WatchProviders> GetWatchProviders(int externalId)
    {
        WatchProviders watchProviders = null;

        var request = new RestRequest($"/tv/{externalId}/watch/providers");
        var response = await _restClient.GetAsync(request);

        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            watchProviders = JsonConvert.DeserializeObject<WatchProviders>(response.Content);
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }
        return watchProviders;
    }
}