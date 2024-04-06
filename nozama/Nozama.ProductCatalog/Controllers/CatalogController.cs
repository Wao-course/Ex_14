using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nozama.ProductCatalog.Data;
using Nozama.Model;

namespace Nozama.ProductCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{

  private readonly ProductCatalogDbContext _dbContext;
  private readonly ILogger<CatalogController> _logger;
  public CatalogController(ProductCatalogDbContext dbContext, ILogger<CatalogController> logger)
  {
    _dbContext = dbContext;
    _logger = logger;
  }

  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<IEnumerable<Product>>> Get([FromQuery] string name = "")
  {
    try
    {
      var result = await _dbContext.Products.Where(p => p.Name.Contains(name)).ToListAsync();

      if (!string.IsNullOrEmpty(name.Trim()))
      {
        var existingStats = await _dbContext.Stats.FirstOrDefaultAsync(s => s.Term == name);
        if (existingStats != null)
        {
          existingStats.Count++;
        }
        else
        {
          await _dbContext.Stats.AddAsync(new StatsEntry
          {
            Term = name,
            Count = 1,
            Timestamp = DateTimeOffset.UtcNow,
            Products = result.ToList()
          });
        }
      }

      await _dbContext.SaveChangesAsync();
      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An error occurred while processing the GET request in CatalogController");
      return StatusCode(500, "An error occurred while processing the request.");
    }
  }
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<Product>> Post(Product product)
  {
    await _dbContext.Products.AddAsync(product);
    await _dbContext.SaveChangesAsync();
    return Created($"{product.ProductId}", product);
  }

}
