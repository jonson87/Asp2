using InMemDb.Data;
using InMemDb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemDb.Models.CartEditViewModel;

namespace InMemDb.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;


        public CartService(ApplicationDbContext context)
        {
            _context = context;
            //_applicationUser = ApplicationUser;
        }

        public async Task<Cart> AddToExistingCart(HttpContext context, Dish dish)
        {
            var cartId = context.Session.GetInt32("Cart");
            var cart = _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient).FirstOrDefault(x => x.CartId == cartId);

            CartItem cartItem = new CartItem();
            List<CartItemIngredient> cartItemIngredients = new List<CartItemIngredient>();

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

            cartItem.Name = dish.Name;
            cartItem.Price = dish.Price;
            cartItem.Quantity = 1;
            cartItem.CartItemIngredient = cartItemIngredients;
            cartItem.CartId = cart.CartId;
            cartItem.DishId = dish.DishId;
            cart.CartItem.Add(cartItem);
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            //_session.SetInt32("Cart", cartId);

            return cart;
        }

        public async Task<Cart> NewCart(Dish dish)
        {
            Cart userCart = new Cart();
            CartItem cartItem = new CartItem();
            List<CartItemIngredient> cartItemIngredients = new List<CartItemIngredient>();
            userCart.CartItem = new List<CartItem>();

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

            cartItem.Name = dish.Name;
            cartItem.Price = dish.Price;
            cartItem.Quantity = 1;
            cartItem.CartItemIngredient = cartItemIngredients;
            cartItem.CartId = userCart.CartId;
            cartItem.DishOriginalPrice = dish.Price;
            cartItem.DishId = dish.DishId;
            userCart.CartItem.Add(cartItem);
            await _context.CartItems.AddAsync(cartItem);
            await _context.Carts.AddAsync(userCart);
            await _context.SaveChangesAsync();
            var cartId = userCart.CartId;
            //_session.SetInt32("Cart", cartId);

            return userCart;
        }

        public async Task<Cart> GetCart(HttpContext context)
        {
            var cartId = context.Session.GetInt32("Cart");
            var cart = await _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient)
                .ThenInclude(x => x.Ingredient).FirstOrDefaultAsync(x => x.CartId == cartId);
            var total = cart.CartItem.Sum(item => item.Price);
            cart.CartTotal = total;
            return cart;
        }

        public async Task<EditCartViewModel> EditCartItemIngredients(EditCartViewModel model)
        {
            foreach (var cii in _context.CartItemIngredients.Where(x => x.CartItemId == model.CartItemId))
            {
                _context.Remove(cii);
            }
            await _context.SaveChangesAsync();

            List<Ingredient> checkedIngredients = _context.Ingredients.Where(i => model.AllIngredients.Where(y => y.Checked)
                .Any(x => x.IngredientId == i.IngredientId)).ToList();

            foreach (var ing in model.AllIngredients)
            {
                foreach (var cIng in checkedIngredients.Where(x => x.IngredientId == ing.IngredientId))
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
            return null;
        }
    }
}
