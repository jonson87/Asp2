using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemDb.Data;
using InMemDb.Models;
using InMemDb.Models.CheckOutViewModel;
using InMemDb.Services;
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
        private readonly IEmailSender _emailSender;

        public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
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
            checkoutViewModel.Cart.CartTotal = cart.CartItem.Sum(x => x.Price);
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
                    CartTotal = checkoutViewModel.Cart.CartTotal,
                    City = checkoutViewModel.User.City,
                    Firstname = checkoutViewModel.User.Firstname,
                    Lastname = checkoutViewModel.User.Lastname,
                    ZipCode = checkoutViewModel.User.ZipCode,
                    Street = checkoutViewModel.User.Street
                };

                await _context.AddAsync(order);
                await _context.SaveChangesAsync();
                await _emailSender.SendEmailAsync(checkoutViewModel.User.Email, "Your Order", "Thank you for using our site. Your order has been placed and will be delivered to you shortly");
                HttpContext.Session.Remove("Cart");
                return View("CheckoutConfirmation", order);
            }
            return View();
        }

        public async Task<IActionResult> CheckoutConfirmation(int orderId)
        {
             var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
             return View(order);
            
        }
    }
}