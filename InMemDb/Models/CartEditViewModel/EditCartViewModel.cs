using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models.CartEditViewModel
{
    public class EditCartViewModel
    {
        public CartItem CartItem { get; set; }
        public int CartItemId { get; set; }
        public List<CartItemIngredient>CartItemIngredients { get; set; }
        public List<Ingredient> AllIngredients { get; set; }
    }
}
