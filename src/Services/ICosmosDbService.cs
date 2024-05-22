namespace MvcMovie
{
    using MvcMovie.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICosmosDbService
    {
        Task<IEnumerable<Movie>> GetItemsAsync(string query);
        Task<Movie> GetItemAsync(string id);
        Task AddItemAsync(Movie item);
        Task UpdateItemAsync(string id, Movie item);
        Task DeleteItemAsync(string id);
    }
}
