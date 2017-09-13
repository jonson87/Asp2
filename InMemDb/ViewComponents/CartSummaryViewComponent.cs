using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemDb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InMemDb.ViewComponents
{
    [ViewComponent(Name = "CartSummary")]
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public CartSummaryViewComponent(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartId = _session.GetInt32("Cart");
            if (cartId == null || cartId == 0)
            {
                return View();
            }
            var cart = await _context.Carts.Include(x => x.CartItem).FirstOrDefaultAsync(x => x.CartId == cartId);
            int dishCount = 0;
            foreach (var dish in _context.Carts.Include(x => x.CartItem).FirstOrDefault(x => x.CartId == cartId).CartItem)
            {
                dishCount++;
            }

            ViewData["CartCount"] = dishCount;
            return View();
        }
    }
}
