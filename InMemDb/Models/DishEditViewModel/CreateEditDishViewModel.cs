using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models.DishEditViewModel
{
    public class CreateEditDishViewModel
    {           
        public string Name { get; set; }
        public int Price { get; set; }
        public Dish Dish { get; set; }
        public List<Ingredient> AllIngredients { get; set; }
    }
}
