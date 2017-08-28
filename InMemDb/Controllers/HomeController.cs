using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InMemDb.Models;
using InMemDb.Data;
using Microsoft.EntityFrameworkCore;

namespace InMemDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var category = _context.Categories.Include(x => x.Dishes).ThenInclude(x => x.DishIngredients).ThenInclude(x=>x.Ingredient).ToList();

            var dishes = _context.Dishes.Include(d => d.DishIngredients).ThenInclude(d => d.Ingredient).ToList();

            return View(category);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
