using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MenusSolution.Models;

namespace MenusSolution.Controllers
{
    public class HomeController : Controller
    {
        //public MenuViewModel MenuViewModel { get; set; }
        public HomeController()
        {
            //TestPartial();
        }
        public IActionResult Index()
        {
            //var menuItems = MenuHelper.GetAllMenuItemsMS();

            //return View(GetMenu(menuItems, null));

           //TestPartial();
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
