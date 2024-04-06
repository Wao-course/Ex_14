using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Nozama.ProductCatalog.Data;
using Nozama.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Nozama.ProductCatalog.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class StatsController : ControllerBase
  {
    private readonly ProductCatalogDbContext _dbContext;

    public StatsController(ProductCatalogDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    // GET: /stats
    [HttpGet]
    public async Task<IActionResult> GetStats()
    {
      try
      {
        // Query the database to retrieve search data
        var searchStats = await _dbContext.Stats
            .ToListAsync();

        // Return the search statistics
        return Ok(searchStats);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"An error occurred while retrieving search statistics: {ex.Message}");
      }
    }


    // DELETE: /stats
    [HttpDelete]
    public async Task<IActionResult> DeleteStats()
    {
      try
      {
        // Remove all search entries from the database
        _dbContext.Stats.RemoveRange(await _dbContext.Stats.ToListAsync());

        // Save changes to the database
        await _dbContext.SaveChangesAsync();

        return Ok("All search entries have been removed from the database.");
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"An error occurred while deleting search entries: {ex.Message}");
      }
    }
  }
}
