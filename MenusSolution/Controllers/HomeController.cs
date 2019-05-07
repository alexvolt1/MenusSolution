using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MenusSolution.Models;
using MenusSolution.Data;
using Microsoft.EntityFrameworkCore;

namespace MenusSolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        //public MenuViewModel MenuViewModel { get; set; }
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            //TestPartial();
            _context.Database.EnsureCreated();
        }
        public async Task<IActionResult> Index()
        {
            //var menuItems = MenuHelper.GetAllMenuItemsMS();

            //return View(GetMenu(menuItems, null));

            //TestPartial();
            //var model = await _context.Menu.ToListAsync();
            //return View(model);

            return View();
        }


        public IActionResult TestPartial()
        {
            //var menuItems = MenuHelper.GetAllMenuItemsEntity();
            //return View(GetMenu(menuItems, null));
            //return PartialView("_Layout", MenuHelper.GetMenu(menuItems, null));

            return null;

            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
