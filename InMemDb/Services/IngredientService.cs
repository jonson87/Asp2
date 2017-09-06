using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemDb.Data;
using InMemDb.Models;

namespace InMemDb.Services
{
    public class IngredientService
    {
        private readonly ApplicationDbContext _context;

        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
            //_applicationUser = ApplicationUser;
        }

        public List<Ingredient> AllIngredients()
        {
            var ingredients = _context.Ingredients.OrderBy(x => x.Name).ToList();

            return ingredients;
        }

    }
}
