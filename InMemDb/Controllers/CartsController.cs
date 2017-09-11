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
        //private readonly ApplicationUser _applicationUser;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager userManager;
        private readonly ICartService _cartService;

        public CartsController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            //_applicationUser = applicationUser;
            //_userManager = userManager;
            //_signInManager = signInManager;
            _cartService = cartService;
            //_applicationUser = ApplicationUser;
        }

        // GET: Carts
        public async Task<IActionResult> Cart(int dishId)
        {
            var total = 0;
            var cartId = (int)HttpContext.Session.GetInt32("Cart");
            var cart = await _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient).ThenInclude(x=>x.Ingredient).FirstOrDefaultAsync(x => x.CartId == cartId);
            foreach (var item in cart.CartItem)
            {
                total += item.Price;
            }
            cart.CartTotal = total;
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int dishId)
        {
            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                await _cartService.NewCart(dishId);
            }
            else
            {
                await _cartService.AddToExistingCart(dishId);
            }
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
            viewModel.AllIngredients = _context.Ingredients.ToList();

            foreach (var aIng in viewModel.AllIngredients)
            {
                foreach (var ing in cartItem.CartItemIngredient)
                {
                    if (ing.IngredientId == aIng.IngredientId)
                    {
                        aIng.Checked = true;
                        if (dish.DishIngredients.Any(x=>x.IngredientId == aIng.IngredientId))
                        {
                            aIng.IngredientPrice = 0;
                        }
                    }
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditCartItemIngredients(EditCartViewModel model)
        {
            foreach (var cii in _context.CartItemIngredients.Where(x=>x.CartItemId == model.CartItemId))
            {
                 _context.Remove(cii);
            }
            await _context.SaveChangesAsync();

            List<Ingredient> checkedIngredients = _context.Ingredients.Where(i => model.AllIngredients.Where(y => y.Checked)
                .Any(x => x.IngredientId == i.IngredientId)).ToList();

            foreach (var ing in model.AllIngredients)
            {
                foreach (var cIng in checkedIngredients.Where(x=>x.IngredientId == ing.IngredientId))
                {
                    cIng.IngredientPrice = ing.IngredientPrice;
                }
            }

            var cartItem = await _context.CartItems.Include(x => x.CartItemIngredient).FirstOrDefaultAsync(x => x.CartItemId == model.CartItemId);
            cartItem.Price = model.CartItem.DishOriginalPrice;

            foreach (var ing in checkedIngredients)
            {
                var cartItemIngredient = new CartItemIngredient()
                {
                    Ingredient = ing,
                    IngredientId = ing.IngredientId,
                    CartItem = model.CartItem,
                    CartItemId = model.CartItemId,
                };
                
                cartItem.Price += ing.IngredientPrice;
                _context.Update(cartItem);
                _context.CartItemIngredients.Add(cartItemIngredient);
            }

            await _context.SaveChangesAsync();
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

            return View("Cart");
        }
    }
}
