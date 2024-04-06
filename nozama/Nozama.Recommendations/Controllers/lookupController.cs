using Microsoft.AspNetCore.Mvc;
using Nozama.Recommendations.Services;
namespace Nozama.Recommendations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LookupController : ControllerBase
    {
        private readonly ProductLookupService _productLookupService;

        public LookupController(ProductLookupService productLookupService)
        {
            _productLookupService = productLookupService;
        }

        [HttpGet("totallookups")]
        public async Task<IActionResult> GetTotalLookups()
        {
            Console.WriteLine("Getting total lookups for products");
            try {
                var totalLookups = await _productLookupService.GetTotalLookupsForProducts();
                return Ok(totalLookups);
            } catch (Exception e) {
                Console.WriteLine($"An error occurred while getting total lookups: {e.Message}");
                return StatusCode(500, "An error occurred while getting total lookups");
            }
            
        }
    }
}
