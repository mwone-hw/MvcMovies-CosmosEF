namespace MvcMovie
{
    using MvcMovie.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICosmosDbService
    {
        Task<IEnumerable<MovieViewModel>> GetItemsAsync(string query);
        Task<MovieViewModel> GetItemAsync(string id);
        Task AddItemAsync(MovieViewModel item);
        Task UpdateItemAsync(string id, MovieViewModel item);
        Task DeleteItemAsync(string id);
    }
}
