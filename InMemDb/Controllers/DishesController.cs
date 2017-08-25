using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InMemDb.Data;
using InMemDb.Models;
using Microsoft.AspNetCore.Http;
using InMemDb.Models.DishEditViewModel;
using Microsoft.AspNetCore.Authorization;

namespace InMemDb.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dishes.Include(d=>d.DishIngredients).ThenInclude(d=>d.Ingredient).ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            CreateEditDishViewModel viewModel = new CreateEditDishViewModel();
            viewModel.AllIngredients = _context.Ingredients.ToList();
            return View(viewModel);
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Price, DishIngredients, AllIngredients")] CreateEditDishViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Dish createdDish = new Dish()
                {
                    Name = viewModel.Name,
                    Price = viewModel.Price,
                };
                _context.Add(createdDish);

                List<Ingredient> allIngredients = _context.Ingredients.Where(i => viewModel.AllIngredients.Where(y => y.Checked).Any(x => x.IngredientId == i.IngredientId)).ToList();

                foreach (var ingredient in allIngredients)
                {                    
                    DishIngredient dishIngredient = new DishIngredient()
                    {
                        Dish = createdDish,
                        Ingredient = ingredient
                    };
                    _context.DishIngredients.Add(dishIngredient);                    
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("index", "home");
        }

        // GET: Dishes/Edit/5
        public IActionResult Edit(int? id)
        {
            var dish = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(x=>x.Ingredient).FirstOrDefault(x => x.DishId == id);

            var model = new CreateEditDishViewModel()
            {
                AllIngredients = _context.Ingredients.ToList(),
                Dish = dish
            };

            foreach (var ing in dish.DishIngredients)
            {
                foreach (var aIng in model.AllIngredients)
                {
                    if (ing.IngredientId == aIng.IngredientId)
                    {
                        aIng.Checked = true;
                    }
                }
            }
            return View(model);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Dish,AllIngredients")] CreateEditDishViewModel model)
        {
            var dish = _context.Dishes
                    .Include(x => x.DishIngredients)
                    .ThenInclude(i => i.Ingredient)
                    .SingleOrDefault(s => s.DishId == model.Dish.DishId);

            //List<Ingredient> allIngredients = _context.Ingredients.ToList();
            List<Ingredient> checkedIngredients = _context.Ingredients.Where(i => model.AllIngredients.Where(y => y.Checked).Any(x => x.IngredientId == i.IngredientId)).ToList();
            List<DishIngredient> dishIngredients = _context.DishIngredients.Where(x=>x.DishId == model.Dish.DishId).ToList();

            foreach (var ing in dishIngredients)
            {
                dish.DishIngredients.Remove(ing);
            }
            await _context.SaveChangesAsync();

            foreach (var ing in checkedIngredients)
            {
                var dishIng = new DishIngredient()
                {
                    Dish = model.Dish,
                    DishId = model.Dish.DishId,
                    Ingredient = ing,
                    IngredientId = ing.IngredientId
                };
                _context.DishIngredients.Add(dishIng);
            }

            dish.Name = model.Dish.Name;
            dish.Price = model.Dish.Price;
            _context.Entry(dish).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Dishes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
