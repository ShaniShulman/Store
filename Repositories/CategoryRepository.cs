using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ManagerApiContext _managerApiContext;
        private readonly IMemoryCache _cache;
        private static readonly string CacheKey = "categoriesCacheKey";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CategoryRepository(ManagerApiContext managerApiContext, IMemoryCache cache)
        {
            _managerApiContext = managerApiContext;
            _cache = cache;
        }

        public async Task<List<Category>> Get()
        {
            if (!_cache.TryGetValue(CacheKey, out List<Category> categories))
            {
                categories = await _managerApiContext.Categories.ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheDuration,
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };

                _cache.Set(CacheKey, categories, cacheEntryOptions);
            }

            return categories;
        }
    }
}