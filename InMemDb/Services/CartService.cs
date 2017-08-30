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
    //public class CartService: ICartService
    //{
    //    private readonly ApplicationDbContext _context;

    //    public CartService(ApplicationDbContext context)
    //    {
    //        _context = context;
    //        //_applicationUser = ApplicationUser;
    //    }

    //    public async Task<Cart> AddToExistingCart(int dishId, string cartId)
    //    {
    //        Cart userCart = new Cart();
    //        CartItem cartItem = new CartItem();
    //        //List<CartItem> cartItems;
    //        List<CartItemIngredient> cartItemIngredients = new List<CartItemIngredient>();

    //        var dish = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(x => x.Ingredient).FirstOrDefault(x => x.DishId == dishId);
    //        //var cartItemIng = new List<CartItemIngredient>();

    //        foreach (var ing in dish.DishIngredients)
    //        {
    //            var cartItemIngredient = new CartItemIngredient()
    //            {
    //                Ingredient = ing.Ingredient,
    //                IngredientId = ing.IngredientId,
    //                CartItem = cartItem,
    //                CartItemId = cartItem.CartItemId
    //            };
    //            cartItemIngredients.Add(cartItemIngredient);
    //        }

    //        cartItem.Dish = dish;
    //        cartItem.Quantity = 1;
    //        cartItem.CartItemIngredient = cartItemIngredients;
    //        cartItem.CartId = userCart.CartId;

    //        await _context.SaveChangesAsync();

    //        return (null);
    //    }

    //    public async Task<Cart> NewCart(int dishId)
    //    {
    //        var cartId = Guid.NewGuid().ToString();

    //        //HttpContext.Session.GetInt32()

    //        Cart userCart = new Cart();
    //        CartItem cartItem = new CartItem();
    //        //List<CartItem> cartItems;
    //        List<CartItemIngredient> cartItemIngredients = new List<CartItemIngredient>();

    //        var dish = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(x => x.Ingredient).FirstOrDefault(x => x.DishId == dishId);
    //        //var cartItemIng = new List<CartItemIngredient>();

    //        foreach (var ing in dish.DishIngredients)
    //        {
    //            var cartItemIngredient = new CartItemIngredient()
    //            {
    //                Ingredient = ing.Ingredient,
    //                IngredientId = ing.IngredientId,
    //                CartItem = cartItem,
    //                CartItemId = cartItem.CartItemId
    //            };
    //            cartItemIngredients.Add(cartItemIngredient);
    //        }

    //        cartItem.Dish = dish;
    //        cartItem.Quantity = 1;
    //        cartItem.CartItemIngredient = cartItemIngredients;
    //        cartItem.CartId = userCart.CartId;

    //        await _context.SaveChangesAsync();

    //        return null;
    //    }

    //}
}
