using System.Security.Claims;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using movie_svc.Controllers;
using movie_svc.DTOs;
using movie_svc.Services;
using movie_svc.ViewModels.Actors;
using movie_svc.ViewModels.Common;
using movie_svc.ViewModels.Favorites;
using movie_svc.ViewModels.Movies;
using movie_svc.ViewModels.Search;
using movie_svc.ViewModels.TVShows;
using movie_svc.ViewModels.TVShows.SeasonsAndEpisodes;
using RestSharp;
using Xunit;

namespace movie.api.Tests;

public class PublicCatalogControllerTests
{
    [Fact]
    public async Task Actor_endpoints_request_the_expected_tmdb_resources()
    {
        var client = new RecordingRestClient();
        var controller = new ActorController(client);

        await controller.GetById(4);
        await controller.ActorSearch("Mary Poppins");
        await controller.ActorTrending();
        await controller.ActorPopular();

        Assert.Equal([
            "/person/4?language=en-US&append_to_response=combined_credits",
            "/search/person?query=Mary Poppins&include_adult=false&language=en-US&page=1",
            "/trending/person/week?language=en-US",
            "/person/popular?language=en-US&page=1"
        ], client.Resources);
    }

    [Fact]
    public async Task Movie_endpoints_request_the_expected_tmdb_resources()
    {
        var client = new RecordingRestClient();
        var controller = new MovieController(client);

        await controller.GetByExternalId(42);
        await controller.MovieSearch("The Matrix");
        await controller.MovieTrending();
        await controller.MoviePopular();
        await controller.GetRecommendations(42, 2);
        await controller.GetReviews(42, 3);

        Assert.Equal([
            "/movie/42?language=en-US", "/movie/42/credits?language=en-US", "/movie/42/watch/providers", "/movie/42/videos",
            "/search/movie?include_adult=false&language=en-US&page=1&query=The Matrix", "/trending/movie/week?language=en-US",
            "/movie/popular?language=en-US&page=1&region=US", "/movie/42/recommendations?language=en-US&page=2", "/movie/42/reviews?language=en-US&page=3"
        ], client.Resources);
    }

    [Fact]
    public async Task Tv_endpoints_request_the_expected_tmdb_resources_and_keep_only_actors()
    {
        var client = new RecordingRestClient();
        var controller = new TvShowController(client);

        var show = await controller.GetByExternalId(8);
        await controller.TvShowSearch("The Office");
        await controller.TvTrending();
        await controller.TvPopular();
        await controller.GetSeasonForTvShow(8, 1);
        await controller.GetEpisodeForTvShowSeason(8, 1, 2);
        await controller.GetReviews(8, 4);

        Assert.Single(show.CastAndCrew.Cast);
        Assert.Equal("Acting", show.CastAndCrew.Cast[0].KnownForDepartment);
        Assert.Equal([
            "/tv/8?language=en-US&append_to_response=aggregate_credits", "/tv/8/watch/providers", "/tv/8/videos",
            "/search/tv?query=The Office&include_adult=false&language=en-US&page=1", "/trending/tv/week?language=en-US", "/tv/popular?language=en-US&page=1",
            "tv/8/season/1", "/tv/8", "/tv/8/season/1/episode/2", "/tv/8", "/tv/8/reviews?language=en-US&page=4"
        ], client.Resources);
    }

    [Fact]
    public async Task Multi_search_preserves_the_requested_page()
    {
        var client = new RecordingRestClient();
        var controller = new SearchController(client);

        await controller.MultiSearch("Star Trek", 5);

        Assert.Equal(["/search/multi?query=Star Trek&include_adult=false&language=en-US&page=5"], client.Resources);
    }
}

public class FavoritesControllerTests
{
    [Fact]
    public async Task Favorites_endpoints_scope_operations_to_the_authenticated_email()
    {
        var service = new RecordingFavoritesService();
        var controller = new FavoritesController(service) { ControllerContext = AuthenticatedContext("viewer@example.com") };
        var favorite = new FavoriteModel { MediaId = "9", MediaTitle = "Example" };

        var addResult = await controller.AddFavorite(favorite);
        var getResult = await controller.GetFavorites();
        var deleteResult = await controller.DeleteFavorite(Guid.Parse("0d19389f-f01e-4170-a3d9-72182895c11e"));

        Assert.Equal("viewer@example.com", favorite.UserEmail);
        Assert.IsType<OkObjectResult>(addResult);
        Assert.IsType<OkObjectResult>(getResult);
        Assert.IsType<OkResult>(deleteResult);
        Assert.Equal("viewer@example.com", service.GetEmail);
        Assert.Equal((Guid.Parse("0d19389f-f01e-4170-a3d9-72182895c11e"), "viewer@example.com"), service.Deleted);
    }

    [Fact]
    public async Task Missing_email_claim_is_rejected()
    {
        var controller = new FavoritesController(new RecordingFavoritesService()) { ControllerContext = AuthenticatedContext(null) };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => controller.GetFavorites());
    }

    private static ControllerContext AuthenticatedContext(string? email) => new()
    {
        HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(email is null ? [] : [new Claim(ClaimTypes.Email, email)], "test")) }
    };
}

public class MapperTests
{
    [Fact]
    public void Favorite_mappers_preserve_every_field_in_both_directions()
    {
        var createdAt = DateTime.UtcNow;
        var model = new FavoriteModel { Id = Guid.NewGuid(), UserEmail = "viewer@example.com", MediaId = "44", MediaTitle = "A Title", MediaImageUrl = "/poster.jpg", MediaType = "movie", CreatedAt = createdAt };

        var dto = Mappers.MapFavoriteVMtoDto(model);
        var mappedModel = Mappers.MapFavoriteDtoToVM(dto);

        Assert.Equal(model.Id, dto.Id);
        Assert.Equal(model.UserEmail, dto.UserEmail);
        Assert.Equal(model.MediaId, dto.MediaId);
        Assert.Equal(model.MediaTitle, dto.MediaTitle);
        Assert.Equal(model.MediaImageUrl, dto.MediaImageUrl);
        Assert.Equal(model.MediaType, dto.MediaType);
        Assert.Equal(model.CreatedAt, dto.CreatedAt);
        Assert.Equal(model.Id, mappedModel.Id);
        Assert.Equal(model.UserEmail, mappedModel.UserEmail);
        Assert.Equal(model.MediaId, mappedModel.MediaId);
        Assert.Equal(model.MediaTitle, mappedModel.MediaTitle);
        Assert.Equal(model.MediaImageUrl, mappedModel.MediaImageUrl);
        Assert.Equal(model.MediaType, mappedModel.MediaType);
        Assert.Equal(model.CreatedAt, mappedModel.CreatedAt);
    }
}

public class RestClientServiceTests
{
    [Fact]
    public async Task Successful_tmdb_response_is_deserialized_and_sent_with_bearer_authentication()
    {
        var handler = new StubHttpMessageHandler(HttpStatusCode.OK, "{\"page\":2,\"total_pages\":3,\"total_results\":1,\"results\":[]}");
        var service = CreateRestClient(handler);

        var result = await service.GetAsync<ReviewResults>(new RestRequest("/movie/8/reviews"));

        Assert.Equal(2, result.Page);
        Assert.Equal("Bearer", handler.Request!.Headers.Authorization!.Scheme);
        Assert.Equal("test-token", handler.Request.Headers.Authorization.Parameter);
        Assert.Equal("application/json", handler.Request.Headers.Accept.Single().MediaType);
    }

    [Fact]
    public async Task Unsuccessful_tmdb_response_throws_an_application_exception()
    {
        var service = CreateRestClient(new StubHttpMessageHandler(HttpStatusCode.NotFound, "{}"));

        var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.GetAsync<ReviewResults>(new RestRequest("/movie/0/reviews")));

        Assert.Contains("NotFound", exception.Message);
    }

    private static RestClientService CreateRestClient(HttpMessageHandler handler)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["TMDB:RestClientRoot"] = "https://tmdb.test/3",
            ["TMDB:MovieApiKey"] = "test-token"
        }).Build();
        return new RestClientService(new HttpClient(handler), configuration);
    }
}

internal sealed class RecordingRestClient : IRestClientService
{
    public List<string> Resources { get; } = [];

    public Task<T> GetAsync<T>(RestRequest request)
    {
        var query = request.Parameters
            .Where(parameter => parameter.Type == ParameterType.QueryString)
            .Select(parameter => $"{parameter.Name}={parameter.Value}");
        Resources.Add($"{request.Resource}{(query.Any() ? $"?{string.Join("&", query)}" : string.Empty)}");
        object result = typeof(T) == typeof(TvShowModel)
            ? new TvShowModel { Name = "Example", CastAndCrew = new CastAndCrewModel { Cast = [new Cast { KnownForDepartment = "Acting" }, new Cast { KnownForDepartment = "Directing" }] } }
            : typeof(T) == typeof(TvSeason) ? new TvSeason()
            : typeof(T) == typeof(TvEpisode) ? new TvEpisode()
            : default(T)!;
        return Task.FromResult((T)result);
    }
}

internal sealed class RecordingFavoritesService : IFavoritesService
{
    public string? GetEmail { get; private set; }
    public (Guid Id, string Email)? Deleted { get; private set; }
    public Task<IEnumerable<FavoriteModel>> GetFavorites(string userEmail) { GetEmail = userEmail; return Task.FromResult<IEnumerable<FavoriteModel>>([]); }
    public Task<FavoriteModel> AddFavorite(FavoriteModel favorite) => Task.FromResult(favorite);
    public Task DeleteFavorite(Guid favoriteId, string userEmail) { Deleted = (favoriteId, userEmail); return Task.CompletedTask; }
}

internal sealed class StubHttpMessageHandler(HttpStatusCode statusCode, string content) : HttpMessageHandler
{
    public HttpRequestMessage? Request { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Request = request;
        return Task.FromResult(new HttpResponseMessage(statusCode) { Content = new StringContent(content) });
    }
}
