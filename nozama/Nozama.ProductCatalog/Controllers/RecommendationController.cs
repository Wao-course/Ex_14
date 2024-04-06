using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Nozama.ProductCatalog.Data;
using Nozama.Model;

namespace Nozama.ProductCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationController : ControllerBase
{
  private readonly ProductCatalogDbContext _dbContext;

  public RecommendationController(ProductCatalogDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Recommendation?>>> Get() => await _dbContext.Recommendations.Include(p => p.Products).AsNoTracking().ToListAsync();

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<Recommendation>> Post(RecommendationRequestDTO recommendation)
  {
    var products = await _dbContext.Products.Where(p => recommendation.ProductIds.Contains(p.ProductId)).ToListAsync();
    var result = new Recommendation {
      Products = products,
      Timestamp = DateTimeOffset.Now,
    };
    await _dbContext.Recommendations.AddAsync(result);
    await _dbContext.SaveChangesAsync();
    return Created($"{recommendation.ProductIds}", result);
  }

  public class RecommendationRequestDTO {
    public List<int>? ProductIds { get; set; }
  }
}
