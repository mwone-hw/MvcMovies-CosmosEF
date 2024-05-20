using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        ICosmosDbService _cosmosDbService;

        public MoviesController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        // GET: Movie
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetItemsAsync("SELECT * FROM c"));
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var movieViewModel = await _context.MovieViewModel
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var movieViewModel = await _cosmosDbService.GetItemAsync(id);

            if (movieViewModel == null)
            {
                return NotFound();
            }

            return View(movieViewModel);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] MovieViewModel movieViewModel)
        {
            //movieViewModel.Id = Guid.NewGuid().ToString();

            if (ModelState.IsValid)
            {
                await _cosmosDbService.AddItemAsync(movieViewModel);
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var movieViewModel = await _context.MovieViewModel.FindAsync(id);
            var movieViewModel = await _cosmosDbService.GetItemAsync(id);
            Console.WriteLine(movieViewModel);
            if (movieViewModel == null)
            {
                return NotFound();
            }
            return View(movieViewModel);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,ReleaseDate,Genre,Price")] MovieViewModel movieViewModel)
        {
            if (id != movieViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(movieViewModel);
                    //await _context.SaveChangesAsync();
                    await _cosmosDbService.UpdateItemAsync(movieViewModel.Id, movieViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieViewModelExists(movieViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var movieViewModel = await _context.MovieViewModel
            //.FirstOrDefaultAsync(m => m.Id == id);
            var movieViewModel = await _cosmosDbService.GetItemAsync(id);

            if (movieViewModel == null)
            {
                return NotFound();
            }

            return View(movieViewModel);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //var movieViewModel = await _context.MovieViewModel.FindAsync(id);
            var movieViewModel = await _cosmosDbService.GetItemAsync(id);
            if (movieViewModel != null)
                //{
                //_context.MovieViewModel.Remove(movieViewModel);
                await _cosmosDbService.DeleteItemAsync(id);
            //}

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieViewModelExists(string id)
        {
            return !_cosmosDbService.GetItemAsync(id).IsNull();
        }
    }
}
