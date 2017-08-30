using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InMemDb.Data;
using InMemDb.Models;
using InMemDb.Services;
using Microsoft.AspNetCore.Http;

namespace InMemDb.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly ICartService _cartService;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
            //_cartService = cartService;
            //_applicationUser = ApplicationUser;
        }

        // GET: Carts
        public async Task<IActionResult> Cart(int dishId)
        {
            var cartId = (int)HttpContext.Session.GetInt32("Cart");
            var cart = _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient).FirstOrDefault(x => x.CartId == cartId);
            //var cart = new Cart();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int dishId)
        {
            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                Cart userCart = new Cart();
                CartItem cartItem = new CartItem();
                List<CartItemIngredient> cartItemIngredients = new List<CartItemIngredient>();
                userCart.CartItem = new List<CartItem>();
                var dish = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(x => x.Ingredient).FirstOrDefault(x => x.DishId == dishId);

                foreach (var ing in dish.DishIngredients)
                {
                    var cartItemIngredient = new CartItemIngredient()
                    {
                        Ingredient = ing.Ingredient,
                        IngredientId = ing.IngredientId,
                        CartItem = cartItem,
                        CartItemId = cartItem.CartItemId
                    };
                    cartItemIngredients.Add(cartItemIngredient);
                }

                cartItem.Dish = dish;
                cartItem.Quantity = 1;
                cartItem.CartItemIngredient = cartItemIngredients;
                cartItem.CartId = userCart.CartId;
                userCart.CartItem.Add(cartItem);
                await _context.CartItems.AddAsync(cartItem);
                await _context.Carts.AddAsync(userCart);
                await _context.SaveChangesAsync();
                var cartId = userCart.CartId;
                HttpContext.Session.SetInt32("Cart", cartId);
                //var newCart = await _cartService.NewCart(dishId);
            }
            else
            {
                var cartId = (int)HttpContext.Session.GetInt32("Cart");
                var cart = _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient).FirstOrDefault(x => x.CartId == cartId);

                CartItem cartItem = new CartItem();
                List<CartItemIngredient> cartItemIngredients = new List<CartItemIngredient>();
                var dish = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(x => x.Ingredient).FirstOrDefault(x => x.DishId == dishId);

                foreach (var ing in dish.DishIngredients)
                {
                    var cartItemIngredient = new CartItemIngredient()
                    {
                        Ingredient = ing.Ingredient,
                        IngredientId = ing.IngredientId,
                        CartItem = cartItem,
                        CartItemId = cartItem.CartItemId
                    };
                    cartItemIngredients.Add(cartItemIngredient);
                }

                cartItem.Dish = dish;
                cartItem.Quantity = 1;
                cartItem.CartItemIngredient = cartItemIngredients;
                cartItem.CartId = cart.CartId;
                cart.CartItem.Add(cartItem);
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetInt32("Cart", cartId);

                //var cart = await _cartService.AddToExistingCart(dishId, cartId.ToString());
            }

            return RedirectToAction("Cart");
        }
    }
}
