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
        //private readonly ICartService _cartService;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
            //_applicationUser = applicationUser;
            //_userManager = userManager;
            //_signInManager = signInManager;
            //_cartService = cartService;
            //_applicationUser = ApplicationUser;
        }

        // GET: Carts
        public async Task<IActionResult> Cart(int dishId)
        {
            var total = 0;
            var cartId = (int)HttpContext.Session.GetInt32("Cart");
            var cart = _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient).ThenInclude(x=>x.Ingredient).FirstOrDefault(x => x.CartId == cartId);
            foreach (var item in cart.CartItem)
            {
                total += item.DishPrice;
            }
            cart.CartTotal = total;
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

                cartItem.DishName = dish.Name;
                cartItem.DishPrice = dish.Price;
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

                cartItem.DishName = dish.Name;
                cartItem.DishPrice = dish.Price;
                cartItem.Quantity = 1;
                cartItem.CartItemIngredient = cartItemIngredients;
                cartItem.CartId = cart.CartId;
                cart.CartItem.Add(cartItem);
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetInt32("Cart", cartId);

                //var cart = await _cartService.AddToExistingCart(dishId, cartId.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditCartItemIngredients(int cartItemId)
        {
            var viewModel = new EditCartViewModel();
            //var cartId = (int)HttpContext.Session.GetInt32("Cart");
            var cartItem = _context.CartItems.Include(x => x.CartItemIngredient).FirstOrDefault(x => x.CartItemId == cartItemId);
            viewModel.CartItem = cartItem;
            viewModel.AllIngredients = _context.Ingredients.ToList();

            foreach (var ing in cartItem.CartItemIngredient)
            {
                foreach (var aIng in viewModel.AllIngredients)
                {
                    if (ing.IngredientId == aIng.IngredientId)
                    {
                        aIng.Checked = true;
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

            List<Ingredient> checkedIngredients = _context.Ingredients.Where(i => model.AllIngredients.Where(y => y.Checked).Any(x => x.IngredientId == i.IngredientId)).ToList();

            foreach (var ing in checkedIngredients)
            {
                var cartItemIngredient = new CartItemIngredient()
                {
                    Ingredient = ing,
                    IngredientId = ing.IngredientId,
                    CartItem = model.CartItem,
                    CartItemId = model.CartItemId
                };
                _context.CartItemIngredients.Add(cartItemIngredient);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Cart");
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
