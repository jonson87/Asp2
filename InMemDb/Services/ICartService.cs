using InMemDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemDb.Models.CartEditViewModel;
using Microsoft.AspNetCore.Http;

namespace InMemDb.Services
{
    public interface ICartService
    {
        Task<Cart> AddToExistingCart(HttpContext context, Dish dish);
        Task<Cart> NewCart(Dish dish);
        Task<Cart> GetCart(HttpContext context);
        Task<EditCartViewModel> EditCartItemIngredients(EditCartViewModel model);
    }
}
