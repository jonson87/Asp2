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
using InMemDb.Models.CartEditViewModel;
using InMemDb.Models.CheckOutViewModel;
using Microsoft.AspNetCore.Identity;

namespace InMemDb.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;

        public CartsController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: Carts
        public async Task<IActionResult> Cart()
        {
            if (HttpContext.Session.GetInt32("Cart") != 0 && HttpContext.Session.GetInt32("Cart") != null)
            {
                return View(await _cartService.GetCart(HttpContext));
            }
            return View("EmptyCart");
        }

        public async Task<IActionResult> AddToCart(int dishId)
        {
            var dish = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(x => x.Ingredient).FirstOrDefault(x => x.DishId == dishId);
            int cartId;

            if (HttpContext.Session.GetInt32("Cart") == 0 || HttpContext.Session.GetInt32("Cart") == null)
            {
                var cart = await _cartService.NewCart(dish);
                cartId = cart.CartId;
            }
            else
            {
                cartId = (int)HttpContext.Session.GetInt32("Cart");
                var updatedCart = await _cartService.AddToExistingCart(HttpContext, dish);
            }
            HttpContext.Session.SetInt32("Cart", cartId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditCartItemIngredients(int cartItemId)
        {
            var viewModel = new EditCartViewModel();
            //var cartId = (int)HttpContext.Session.GetInt32("Cart");
            var cartItem = await _context.CartItems.Include(x => x.CartItemIngredient).FirstOrDefaultAsync(x => x.CartItemId == cartItemId);
            var dish = await _context.Dishes.Include(x=>x.DishIngredients).FirstOrDefaultAsync(x => x.DishId == cartItem.DishId);
            cartItem.DishOriginalPrice = dish.Price;
            viewModel.CartItem = cartItem;
            viewModel.AllIngredients = _context.Ingredients.OrderBy(x=>x.Name).ToList();
            var cartItemIngredients = new List<CartItemIngredient>();
            foreach (var aIng in viewModel.AllIngredients)
            {
                var cartItemIngredient = new CartItemIngredient();

                if (cartItem.CartItemIngredient.Any(x => x.IngredientId == aIng.IngredientId))
                {
                    cartItemIngredient.Checked = true;
                    cartItemIngredient.CartItemIngredientPrice = 0;
                    cartItemIngredient.Ingredient = aIng;
                    cartItemIngredient.CartItem = cartItem;
                    cartItemIngredient.IngredientId = aIng.IngredientId;
                    cartItemIngredient.CartItemId = cartItem.CartItemId;
                }
                else
                {
                    cartItemIngredient.CartItemIngredientPrice = aIng.IngredientPrice;
                    cartItemIngredient.Checked = false;
                    cartItemIngredient.Ingredient = aIng;
                    cartItemIngredient.CartItem = cartItem;
                    cartItemIngredient.IngredientId = aIng.IngredientId;
                    cartItemIngredient.CartItemId = cartItem.CartItemId;
                }
                //foreach (var ing in cartItem.CartItemIngredient)
                //{
                //    if ()
                //    {
                //        cartItemIngredient.Checked = true;
                //        cartItemIngredient.CartItemIngredientPrice = 0;
                //        cartItemIngredient.Ingredient = aIng;
                //        cartItemIngredient.CartItem = cartItem;
                //        cartItemIngredient.IngredientId = aIng.IngredientId;
                //        cartItemIngredient.CartItemId = cartItem.CartItemId;
                //    }
                //    else
                //    {
                //    cartItemIngredient.CartItemIngredientPrice = aIng.IngredientPrice;
                //    cartItemIngredient.Checked = false;
                //    cartItemIngredient.Ingredient = aIng;
                //    cartItemIngredient.CartItem = cartItem;
                //    cartItemIngredient.IngredientId = aIng.IngredientId;
                //    cartItemIngredient.CartItemId = cartItem.CartItemId;
                //    }
                //}
                cartItemIngredients.Add(cartItemIngredient);
            }
            viewModel.CartItemIngredients = cartItemIngredients;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditCartItemIngredients(EditCartViewModel model)
        {
            await _cartService.EditCartItemIngredients(model);
            return RedirectToAction("Cart");
        }

        public async Task<bool> OriginalDishIngredient(int dishId, int ingredientId)
        {
            return await _context.DishIngredients.AnyAsync(di => di.DishId == dishId && di.IngredientId == ingredientId);
        }

        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            if (cartItemId == 0)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .SingleOrDefaultAsync(m => m.CartItemId == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Cart");
        }
    }
}
