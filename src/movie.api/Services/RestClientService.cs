using Newtonsoft.Json;

namespace movie_svc.Services;

using RestSharp;
using System.Net.Http;

public class RestClientService : IRestClientService
{
    private readonly RestClient _restClient;

    public RestClientService(HttpClient httpClient, IConfiguration configuration)
    {
        // Use HttpClient from the factory
        var options = new RestClientOptions(configuration["TMDB:RestClientRoot"]);
        _restClient = new RestClient(httpClient, options);
        
        // Set default headers that will apply to all requests
        _restClient.AddDefaultHeader("Authorization", $"Bearer {configuration["TMDB:MovieApiKey"]}");
        _restClient.AddDefaultHeader("Accept", "application/json");
    }

    public async Task<T> GetAsync<T>(RestRequest request)
    {
        T result = default!;
        
        var response = await _restClient.GetAsync(request);
        
        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = JsonConvert.DeserializeObject<T>(response.Content);
        } else {
            throw new ApplicationException($"Error calling external movie API - {response.StatusCode}");
        }
        
        return result;
    }
}
