using InMemDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Services
{
    public interface ICartService
    {
        Task<Cart> AddToExistingCart(int dishId);
        Task<Cart> NewCart(int dishId);
    }
}
