namespace MvcMovie
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MvcMovie.Models;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Fluent;
    using Microsoft.Extensions.Configuration;

    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }
        
        public async Task AddItemAsync(Movie movie)
        {
            await this._container.CreateItemAsync<Movie>(movie, new PartitionKey(movie.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Movie>(id, new PartitionKey(id));
        }

        public async Task<Movie> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Movie> response = await this._container.ReadItemAsync<Movie>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch(CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            { 
                return null;
            }

        }

        public async Task<IEnumerable<Movie>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Movie>(new QueryDefinition(queryString));
            List<Movie> results = new List<Movie>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Movie item)
        {
            await this._container.UpsertItemAsync<Movie>(item, new PartitionKey(id));
        }
    }
}
