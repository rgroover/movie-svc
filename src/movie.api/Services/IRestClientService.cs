using RestSharp;

namespace movie_svc.Services;

public interface IRestClientService
{
    public Task<T> GetAsync<T>(RestRequest request);
}