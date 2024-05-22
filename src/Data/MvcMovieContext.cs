using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        //CosmosAdaptation - Specifying containers and partition key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Container
            modelBuilder.Entity<Movie>()
                .ToContainer("Movies");
            #endregion

            #region PartitionKey
            modelBuilder.Entity<Movie>()
                .HasPartitionKey(o => o.PartitionKey);
            #endregion
        }

        public DbSet<MvcMovie.Models.Movie> Movie { get; set; } = default!;
    }
}
