using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int DishOriginalPrice { get; set; }
        public int DishId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public List<CartItemIngredient> CartItemIngredient { get; set; }
        public bool Modified { get; set; }
    }
}
