namespace MvcMovie
{
    using Microsoft.Azure.Cosmos;
    using MvcMovie.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public async Task AddItemAsync(MovieViewModel movie)
        {
            await this._container.CreateItemAsync<MovieViewModel>(movie, new PartitionKey(movie.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<MovieViewModel>(id, new PartitionKey(id));
        }

        public async Task<MovieViewModel> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<MovieViewModel> response = await this._container.ReadItemAsync<MovieViewModel>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<MovieViewModel>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<MovieViewModel>(new QueryDefinition(queryString));
            List<MovieViewModel> results = new List<MovieViewModel>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, MovieViewModel item)
        {
            await this._container.UpsertItemAsync<MovieViewModel>(item, new PartitionKey(id));
        }
    }
}
