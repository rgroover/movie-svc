using Microsoft.Azure.Cosmos;
using movie_svc.DTOs;
using movie_svc.ViewModels.Favorites;

namespace movie_svc.Services;

public class FavoritesService : IFavoritesService
{
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger<FavoritesService> _logger;
    private readonly Lazy<Task<Container>> _container;
    
    public FavoritesService(ILogger<FavoritesService> logger, CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        _logger = logger;
        _container = new Lazy<Task<Container>>(InitializeContainer);
    }

    private async Task<Container> InitializeContainer()
    {
        var containerProperties = new ContainerProperties("favorites", "/user_email");
        var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("movie-app");
        var container = await database.Database.CreateContainerIfNotExistsAsync(containerProperties);     
        return container.Container;
    }

    public async Task DeleteFavorite(Guid favoriteId, string userEmail)
    {
        var container = await _container.Value;
        ItemResponse<FavoriteDto> response = await container.DeleteItemAsync<FavoriteDto>(favoriteId.ToString(), new PartitionKey(userEmail));
        _logger.LogDebug("Deleted favorite {FavoriteId} with status {StatusCode}", favoriteId, response.StatusCode);
    }

    public async Task<FavoriteModel> AddFavorite(FavoriteModel favorite)
    {
        var container = await _container.Value;
        var favCount = await GetFavoritesCount(favorite.UserEmail);
        if (favCount >= 100)
        {
            throw new ApplicationException("User has reached the maximum number of favorites = 100");
        }
        var favoriteDto = Mappers.MapFavoriteVMtoDto(favorite);
        favoriteDto.CreatedAt = DateTime.UtcNow;
        favoriteDto.Id = Guid.NewGuid();
        ItemResponse<FavoriteDto> response = await container.UpsertItemAsync(favoriteDto, new PartitionKey(favoriteDto.UserEmail));
        return Mappers.MapFavoriteDtoToVM(response.Resource);
    }

    public async Task<int> GetFavoritesCount(string userEmail)
    {
        var container = await _container.Value;
        QueryDefinition queryDefinition = new QueryDefinition("SELECT VALUE COUNT(1) FROM c");
        
        int count = 0;
        var iterator = container.GetItemQueryIterator<int>(queryDefinition, requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userEmail) });
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            count += response.Resource.FirstOrDefault();
        }
        return count;
    }

    public async Task<IEnumerable<FavoriteModel>> GetFavorites(string userEmail)
    {
        var container = await _container.Value;
        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c");

        FeedIterator<FavoriteDto> queryResultSetIterator = container.GetItemQueryIterator<FavoriteDto>(queryDefinition, requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userEmail) });

        List<FavoriteDto> favorites = new List<FavoriteDto>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<FavoriteDto> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            favorites.AddRange(currentResultSet);
        }

        return favorites.Select(Mappers.MapFavoriteDtoToVM).ToList();
    }
}
