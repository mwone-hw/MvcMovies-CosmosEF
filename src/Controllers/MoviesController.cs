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

        // GET: Movies
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetItemsAsync("SELECT * FROM c"));
        }

        // GET: Movies/Details/5
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

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movieViewModel)
        {
            if (ModelState.IsValid)
            {
                //movieViewModel.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddItemAsync(movieViewModel);
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Movies/Edit/5
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

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movieViewModel)
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

        // GET: Movies/Delete/5
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

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //var movieViewModel = await _context.MovieViewModel.FindAsync(id);
            var movieViewModel = await _cosmosDbService.GetItemAsync(id);
            if (movieViewModel != null)
            {
                //_context.MovieViewModel.Remove(movieViewModel);
                await _cosmosDbService.DeleteItemAsync(id);
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieViewModelExists(string id)
        {
            return !_cosmosDbService.GetItemAsync(id).IsNull();
        }
    }
}
