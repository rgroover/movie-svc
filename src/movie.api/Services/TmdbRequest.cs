using RestSharp;

namespace movie_svc.Services;

public static class TmdbRequest
{
    public static RestRequest Get(string resource, params (string Name, object Value)[] queryParameters)
    {
        var request = new RestRequest(resource);
        foreach (var (name, value) in queryParameters)
        {
            var stringValue = value is bool boolean ? boolean.ToString().ToLowerInvariant() : value.ToString();
            request.AddQueryParameter(name, stringValue);
        }

        return request;
    }
}
