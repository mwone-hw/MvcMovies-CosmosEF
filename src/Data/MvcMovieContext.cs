using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
	public class MvcMovieContext : DbContext
	{
		public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
			: base(options)
		{
		}

		//CosmosAdaptation - Specifying containers and partition key
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Set default container
			modelBuilder.HasDefaultContainer("Movies"); 

			//Map Movie entity to Movies container
			#region Container
			modelBuilder.Entity<Movie>()
				.ToContainer("Movies");	
			#endregion

			//Remove prefix in Id
			//Id will be stored as "Movies|<GUID>" if this is not called
			#region NoDiscriminator
			modelBuilder.Entity<Movie>()
				.HasNoDiscriminator();
			#endregion

			//Set Id(id) as Partition Key
			#region PartitionKey
			modelBuilder.Entity<Movie>()
				.HasPartitionKey(m => m.Id);
			#endregion
		}

		public DbSet<Movie> Movie { get; set; } = default!;
	}
}
