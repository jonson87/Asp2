using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models
{
    public class CartItemIngredient
    {
        public int CartItemIngredientId { get; set; }

        public int CartItemId { get; set; }

        public CartItem CartItem { get; set; }

        public int IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
