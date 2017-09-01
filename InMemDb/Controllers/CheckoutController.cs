using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemDb.Data;
using InMemDb.Models;
using InMemDb.Models.CheckOutViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InMemDb.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationUser _applicationUser;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager userManager;
        //private readonly ICartService _cartService;

        public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ApplicationUser applicationUser)
        {
            _context = context;
            _applicationUser = applicationUser;
            _userManager = userManager;
            //_signInManager = signInManager;
            //_cartService = cartService;
            //_applicationUser = ApplicationUser;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cartId = HttpContext.Session.GetInt32("Cart");

            var cart = _context.Carts.Include(x => x.CartItem).ThenInclude(x => x.CartItemIngredient).FirstOrDefault(x => x.CartId == cartId);
            CheckoutViewModel checkoutViewModel = new CheckoutViewModel();
            if (_applicationUser == null)
            {

                checkoutViewModel.Cart = cart;
                return View(checkoutViewModel);
            }
            checkoutViewModel.User = await _userManager.GetUserAsync(User);
            ;
            checkoutViewModel.Cart = cart;
            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel checkoutViewModel)
        {

            return null;
        }
    }
}