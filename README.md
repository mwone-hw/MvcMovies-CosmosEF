Followed [MVC project guide](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-8.0&tabs=visual-studio) till [step 4 Model Scaffolding](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-8.0&tabs=visual-studio).

Instead of using SQL server as Database provider, this project used CosmosDB to investigate difference between SQL and Cosmos in EF.

Main difference: 
- Container and Partition Key configuration in [DBContext](https://github.com/mwone-hw/MvcMovies-CosmosEF/blob/master/MvcMovie/Data/MvcMovieContext.cs#L17)
- Connection setup in [Program](https://github.com/mwone-hw/MvcMovies-CosmosEF/blob/master/MvcMovie/Program.cs#L6)

Note: The CosmosDBService classes are irrelevant when using EF
