using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Nozama.ProductCatalog.Services
{
    public class ProductCatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public ProductCatalogService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
            var uri = Environment.GetEnvironmentVariable("PRODUCT_CATALOG_SERVICE_URI");
            if (uri is not null)
            {
                _httpClient.BaseAddress = new Uri(uri);
            }
        }

        public async Task<IEnumerable<Product>?> GetProducts()
        {
            // Try to retrieve products from cache
            if (_cache.TryGetValue<IEnumerable<Product>>("ProductsCache", out var cachedProducts))
            {
                return cachedProducts;
            }

            // If not found in cache, retrieve products from the API
            var products = await _httpClient.GetFromJsonAsync<IEnumerable<Product>?>("/productcatalog");

            // Cache the retrieved products for future requests
            if (products != null)
            {
                _cache.Set("ProductsCache", products, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
            }

            return products;
        }
    }
}
