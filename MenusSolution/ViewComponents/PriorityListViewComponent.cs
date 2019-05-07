using MenusSolution.Data;
using MenusSolution.Helpers;
using MenusSolution.Models;
using MenusSolution.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenusSolution.ViewComponents
{
    public class PriorityListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public PriorityListViewComponent(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(
        int maxPriority, bool isDone)
        {
            IEnumerable<Menu> cacheEntry;

            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = await _context.Menu.ToListAsync();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(15));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }

            return View(cacheEntry);
        }

    }
}
