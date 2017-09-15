using System;
using System.Collections.Generic;
using System.Text;
using InMemDb.Data;
using InMemDb.Models;
using InMemDb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.ObjectPool;

namespace BennysPizza.UnitTests
{
    public class PricingTests
    {
        private readonly IServiceProvider _serviceProvider;

        public PricingTests()
        {
            var efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>()
                .AddDbContext<ApplicationDbContext>(b => b
                    .UseInMemoryDatabase("Scratch")
                    .UseInternalServiceProvider(efServiceProvider))
                .AddTransient<IHostingEnvironment, HostingEnvironment>()
                .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IEmailSender, EmailSender>()
                .AddTransient<ICartService, CartService>()
                .AddSession()
                .AddMvc();

            _serviceProvider = services.BuildServiceProvider();
            services.AddSingleton(_serviceProvider);

            var ingredientTomatoe = new Ingredient{Name = "Tomatoe",IngredientPrice = 5};
            var ingredientCheese = new Ingredient{Name = "Cheese",IngredientPrice = 5};

            var pizzaCategory = new Category {CategoryId=1, Name = "Pizza"};
            
            //_Margeritha = new Dish{DishId=1, Name = "Margeritha",Price = 79, Category = pizzaCategory, CategoryId = pizzaCategory.CategoryId };
            //var margerithaCheese = new DishIngredient{Ingredient = ingredientCheese,Dish = _Margeritha };
            //var margerithaTomatoe = new DishIngredient{Ingredient = ingredientTomatoe,Dish = _Margeritha };

            //_Margeritha.DishIngredients = new List<DishIngredient> {margerithaCheese, margerithaTomatoe};

            var user = new ApplicationUser
            {
                Firstname = "Erik",
                Lastname = "Jonson",
                Email = "Email@email.com",
                City = "Habo",
                Street = "Gatan",
                ZipCode = "55633",
                UserName = "Email@email.com"
            };
            //_newContext.Add(user);
            //_newContext.SaveChanges();
        }

        [Fact]
        public void Test_Price_On_Dish()
        {
            var httpContext = new DefaultHttpContext();

            //httpContext.Session.SetString("Session", cartId);

            var ingredientTomatoe = new Ingredient { Name = "Tomatoe", IngredientPrice = 5 };
            var ingredientCheese = new Ingredient { Name = "Cheese", IngredientPrice = 5 };

            var pizzaCategory = new Category { CategoryId = 1, Name = "Pizza" };

            var margeritha = new Dish { DishId = 1, Name = "Margeritha", Price = 79, Category = pizzaCategory, CategoryId = pizzaCategory.CategoryId };
            var margerithaCheese = new DishIngredient { Ingredient = ingredientCheese, Dish = margeritha };
            var margerithaTomatoe = new DishIngredient { Ingredient = ingredientTomatoe, Dish = margeritha };

            margeritha.DishIngredients = new List<DishIngredient> { margerithaCheese, margerithaTomatoe };

            var cartService = _serviceProvider.GetService<ICartService>();

            var cart = cartService.NewCart(margeritha).Result;

            var price = cart.CartItem[0].Price;

            Assert.Equal(79, price);
        }

        //[Fact]
        //public void Dish_Price_With_Extra_Ingredient()
        //{
            
        //}

        //public Dish CreateDish()
        //{
        //    var margeritha = new Dish { Name = "Margeritha", Price = 79 };
        //    return margeritha;
        //}
    }
}
