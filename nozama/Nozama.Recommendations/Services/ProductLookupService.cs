using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nozama.Model; // Adjust the namespace as per your project structure

namespace Nozama.Recommendations.Services
{
    public class ProductLookupService
    {
        private readonly RecommendationsDbContext _dbContext;

        public ProductLookupService(RecommendationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dictionary<string, int>> GetTotalLookupsForProducts()
        {
            var totalLookups = new Dictionary<string, int>();

            // Query the database to get the total number of lookups for each product
            var statsEntries = await _dbContext.Stats.ToListAsync();

            foreach (var statsEntry in statsEntries)
            {
                // Use the term directly from the StatsEntry object
                var term = statsEntry.Term;

                // Check if the term already exists in the dictionary
                if (totalLookups.ContainsKey(term))
                {
                    totalLookups[term] += statsEntry.Count;
                }
                else
                {
                    totalLookups[term] = statsEntry.Count;
                }
            }

            return totalLookups;
        }
    }
}
