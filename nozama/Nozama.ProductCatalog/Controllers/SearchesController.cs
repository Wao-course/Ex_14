using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Nozama.Model; // Make sure to import the appropriate namespace for your models
using Nozama.ProductCatalog.Data;

namespace Nozama.ProductCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ProductCatalogDbContext _dbContext;
          private readonly ILogger<SearchController> _logger;


        public SearchController(ProductCatalogDbContext dbContext,ILogger<SearchController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("Allsearches")]
        public async Task<ActionResult<IEnumerable<Search>>> GetSearches()
        {
            return await _dbContext.Searches.ToListAsync();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteSearches()
        {
            try
            {
                _dbContext.Searches.RemoveRange(_dbContext.Searches);
                await _dbContext.SaveChangesAsync();
                return Ok("All searches have been removed from the database.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting searches: {ex.Message}");
            }
        }


        [HttpGet("latestSearches")]
        public async Task<ActionResult<IEnumerable<Search>>> GetLatestSearches()
        {
            var latestSearches = await _dbContext.Searches
                .GroupBy(s => s.Term)
                .Select(g => new { Term = g.Key, Count = g.Count() }) // Calculate the count of elements in each group
                .OrderByDescending(g => g.Count) // Order by the count of elements in each group
                .Take(100)
                .ToListAsync();

            // Flatten the grouped results to get individual search records
            var flattenedSearches = latestSearches.SelectMany(g => _dbContext.Searches.Where(s => s.Term == g.Term)).ToList();

            return Ok(flattenedSearches);
        }


        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Product>>> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search term cannot be empty.");
            }

            var products = await _dbContext.Products
                .AsNoTracking()
                .Where(p => p.Name.Contains(name))
                .ToListAsync();

            if (products.Any())
            {
                try
                {
                    // Log the search query
                    await LogSearch(name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Product Doesn't Exist");
                }
            }
            else
            {
                _logger.LogWarning("No products found for search term: {name}", name);
            }

            return Ok(products);
        }

        [NonAction]
        public async Task LogSearch(string searchTerm)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Name.Contains(searchTerm));

            if (product != null)
            {
                // Create a new Search object with the search term, productId, and timestamp
                var search = new Search
                {
                    Term = searchTerm,
                    ProductId = product.ProductId,
                    Timestamp = DateTimeOffset.Now
                };

                // Add the search object to the database context
                _dbContext.Searches.Add(search);

                // Save changes to the database asynchronously
                await _dbContext.SaveChangesAsync();
            }
        }

    }


}
