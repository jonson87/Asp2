using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemDb.Data;
using InMemDb.Models;
using InMemDb.Models.CheckOutViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InMemDb.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> Checkout()
        {
            var cartId = HttpContext.Session.GetInt32("Cart");

            var cart = _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient).FirstOrDefault(x => x.CartId == cartId);
            var checkoutViewModel = new CheckoutViewModel();
            if (await _userManager.GetUserAsync(User) == null)
            {
                checkoutViewModel.Cart = cart;
                return View(checkoutViewModel);
            }
            checkoutViewModel.User = await _userManager.GetUserAsync(User);
            
            checkoutViewModel.Cart = cart;
            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel checkoutViewModel)
        {
            //var cartId = HttpContext.Session.GetInt32("Cart");
            if (ModelState.IsValid)
            {
                var order = new Order()
                {
                    Cart = _context.Carts.FirstOrDefault(x=>x.CartId == checkoutViewModel.Cart.CartId),
                    User = checkoutViewModel.User,
                    TimeOfOrder = DateTime.Now,
                };

                await _context.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            return View();
        }
    }
}