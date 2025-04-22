using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;
using movie_svc.DTOs;
using movie_svc.ViewModels.Favorites;

namespace movie_svc.Services;

public class FavoritesService : IFavoritesService
{
    private readonly CosmosClient _cosmosClient;
    private Container _container;
    private readonly ILogger<FavoritesService> _logger;
    
    public FavoritesService(ILogger<FavoritesService> logger, CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
        _logger = logger;
    }

    public async Task Init()
    {
        var containerProperties = new ContainerProperties("favorites", "/user_email");
        var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("movie-app");
        var container = await database.Database.CreateContainerIfNotExistsAsync(containerProperties);     
        _container = container.Container;
    }

    public async Task DeleteFavorite(Guid favoriteId, string userEmail)
    {
        try
        {
            await Init();
            ItemResponse<FavoriteDto> response = await _container.DeleteItemAsync<FavoriteDto>(favoriteId.ToString(), new PartitionKey(userEmail));
            _logger.LogDebug(response.StatusCode.ToString());
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Cosmos DB Error: {ex.Message}");
        }
    }

    public async Task<FavoriteModel> AddFavorite(FavoriteModel favorite)
    {
        try
        {
            await Init();
            var favCount = await GetFavoritesCount(favorite.UserEmail);
            if (favCount >= 100)
            {   
                // need a global error handler to return something useful to the client instead of a 500
                throw new ApplicationException("User has reached the maximum number of favorites = 100");   
            }
            var favoriteDto = Mappers.MapFavoriteVMtoDto(favorite);
            favoriteDto.CreatedAt = DateTime.UtcNow;
            favoriteDto.Id = Guid.NewGuid();
            ItemResponse<FavoriteDto> response = await _container.UpsertItemAsync(favoriteDto, new PartitionKey(favoriteDto.UserEmail));
            FavoriteModel model = Mappers.MapFavoriteDtoToVM(response.Resource);
            return model; // Return the newly created item
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Cosmos DB Error: {ex.Message}");
            return null;
        }
    }

    public async Task<int> GetFavoritesCount(string userEmail)
    {
        await Init();
        var sqlQueryText = $"SELECT VALUE COUNT(1) FROM c WHERE c.user_email = '{userEmail}'";

        QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
        
        int count = 0;
        var iterator = _container.GetItemQueryIterator<int>(queryDefinition);
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            count += response.Resource.FirstOrDefault();
        }
        return count;
    }

    public async Task<IEnumerable<FavoriteModel>> GetFavorites(string userEmail)
    {
        try
        {
            await Init();
            var sqlQueryText = $"SELECT * FROM c WHERE c.user_email = '{userEmail}'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

            FeedIterator<FavoriteDto> queryResultSetIterator =
                _container.GetItemQueryIterator<FavoriteDto>(queryDefinition);

            List<FavoriteDto> favorites = new List<FavoriteDto>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<FavoriteDto> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                favorites.AddRange(currentResultSet);
            }

            List<FavoriteModel> favoritesModelList = new List<FavoriteModel>();
            foreach (FavoriteDto favorite in favorites)
            {
                favoritesModelList.Add(Mappers.MapFavoriteDtoToVM(favorite));
            }

            return favoritesModelList;
        }
        catch (CosmosException cosmosException)
        {
            Console.WriteLine("Cosmos Exception >>>>>" + cosmosException.ToString());
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Generic Exception >>>>>" + ex.ToString());
            return null;
        }
    }
}