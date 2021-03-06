﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MenusSolution.Data;
using MenusSolution.Models;
using MenusSolution.Models.ViewModels;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using MenusSolution.Helpers;
using System.Data.SqlClient;

namespace MenusSolution.Controllers
{
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public MenusController(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            IEnumerable<Menu> cacheEntry;

            //IEnumerable<Menu> cachedMenu;

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



            //return View(await _context.Menu.ToListAsync());
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu
                .FirstOrDefaultAsync(m => m.ID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ParentID,Content,IconClass,Href,Order")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,ParentID,Content,IconClass,Href,Order")] Menu menu)
        {
            if (id != menu.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu
                .FirstOrDefaultAsync(m => m.ID == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var menu = await _context.Menu.FindAsync(id);
            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(string id)
        {
            return _context.Menu.Any(e => e.ID == id);
        }


        public IActionResult RenderString()
        {
            return View("String", GetMenuString());
        }
        public IActionResult Model()
        {
            var menuItems = GetAllMenuItemsMS();
            return View("Model", MenuHelper.GetMenu(menuItems, null));
        }

        private string GetMenuString()
        {
            var menuItems = MenuHelper.GetAllMenuItems();

            var builder = new StringBuilder();
            builder.Append("<ul class=\"sidebar-menu\" data-widget=\"tree\">");
            builder.Append(GetMenuLiString(menuItems, null));
            builder.Append("</ul>");
            return builder.ToString();
        }

        private string GetMenuLiString(IList<Menu> menuList, string parentId)
        {
            var children = MenuHelper.GetChildrenMenu(menuList, parentId);

            if (children.Count <= 0)
            {
                return "";
            }

            var builder = new StringBuilder();

            foreach (var item in children)
            {
                var childStr = GetMenuLiString(menuList, item.ID);
                if (!string.IsNullOrWhiteSpace(childStr))
                {
                    builder.Append("<li class=\"treeview\">");
                    builder.Append("<a href=\"#\">");
                    builder.AppendFormat("<i class=\"{0}\"></i> <span>{1}</span>", item.IconClass, item.Content);
                    builder.Append("<span class=\"pull-right-container\">");
                    builder.Append("<i class=\"fa fa-angle-left pull-right\"></i>");
                    builder.Append("</span>");
                    builder.Append("</a>");
                    builder.Append("<ul class=\"treeview-menu\">");
                    builder.Append(childStr);
                    builder.Append("</ul>");
                    builder.Append("</li>");
                }
                else
                {
                    builder.Append("<li class=\"treeview\">");
                    builder.AppendFormat("<a href=\"{0}\">", item.Href);
                    builder.AppendFormat("<i class=\"{0}\"></i> <span>{1}</span>", item.IconClass, item.Content);
                    builder.Append("</a>");
                    builder.Append("</li>");
                }
            }

            return builder.ToString();
        }

        public IList<Menu> GetAllMenuItemsMS()
        {
            MenuHelper mh = new MenuHelper();
            List<Menu> menuList = new List<Menu>();
            IList<Menu> cacheEntry;


            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {

                using (SqlConnection cn = new SqlConnection("Server=DESKTOP-UKJJB1E\\WINSQL;Database=TestMenu;Trusted_Connection=True;MultipleActiveResultSets=true;"))
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [TestMenu].[dbo].[Menu]", cn);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Menu menu = new Menu();
                        menu.ID = (string)reader["ID"];
                        menu.ParentID = (reader["ParentID"] == DBNull.Value) ? null : reader["ParentID"].ToString();
                        menu.Content = (string)reader["Content"];
                        menu.IconClass = (string)reader["IconClass"];
                        menu.Href = (string)reader["Href"];

                        menuList.Add(menu);
                    }
                    cacheEntry = menuList;

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(15));

                    // Save data in cache.
                    _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
                    cn.Close();
                }
            }
            return cacheEntry;
        }
    }
}
