using InMemDb.Data;
using InMemDb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private ISession _session => _contextAccessor.HttpContext.Session;


        public CartService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            //_applicationUser = ApplicationUser;
        }

        public async Task<Cart> AddToExistingCart(int dishId)
        {
            var cartId = _session.GetInt32("Cart");
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

            return (null);
        }

        public async Task<Cart> NewCart(int dishId)
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
            _session.SetInt32("Cart", cartId);

            return null;
        }

        public async Task<Cart> GetCart(int dishId)
        {
            var cartId = _session.GetInt32("Cart");
            var cart = await _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient)
                .ThenInclude(x => x.Ingredient).FirstOrDefaultAsync(x => x.CartId == cartId);
            var total = cart.CartItem.Sum(item => item.Price);
            cart.CartTotal = total;
            return cart;
        }

    }
}
