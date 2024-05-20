using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models;

public class MovieViewModel
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Title { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }
    //public string PartitionKey { get; set; }
}