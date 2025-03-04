using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _icategoryService;
        private readonly IMapper _imapper;
        private readonly IMemoryCache _cache;
        private static readonly string CacheKey = "categoriesCacheKey";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CategoriesController(ICategoryService icategoryService, IMapper imapper, IMemoryCache cache)
        {
            _icategoryService = icategoryService;
            _imapper = imapper;
            _cache = cache;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoryDTO>>> Get()
        {
            if (!_cache.TryGetValue(CacheKey, out IEnumerable<GetCategoryDTO> categoriesDTO))
            {
                IEnumerable<Category> categories = await _icategoryService.Get();
                categoriesDTO = _imapper.Map<IEnumerable<Category>, IEnumerable<GetCategoryDTO>>(categories);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheDuration,
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };

                _cache.Set(CacheKey, categoriesDTO, cacheEntryOptions);
            }

            if (categoriesDTO != null)
                return Ok(categoriesDTO);
            else
                return NoContent();
        }
    }
}