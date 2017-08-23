using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InMemDb.Data;

namespace InMemDb.Controllers
{
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dishes = _context.Dishes.ToList();

            return View(dishes);
        }
    }
}