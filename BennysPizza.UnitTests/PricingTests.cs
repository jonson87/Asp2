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
using InMemDb.Models.CartEditViewModel;
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
            var _fakeContext = _serviceProvider.GetService<ApplicationDbContext>();

            var ingredientTomatoe = new Ingredient{Name = "Tomatoe",IngredientPrice = 5};
            var ingredientCheese = new Ingredient{Name = "Cheese",IngredientPrice = 5};
            var ingredientShrimp = new Ingredient { Name = "Shrimp", IngredientPrice = 5 };
            _fakeContext.Ingredients.Add(ingredientTomatoe);
            _fakeContext.Ingredients.Add(ingredientCheese);
            _fakeContext.Ingredients.Add(ingredientShrimp);


            var pizzaCategory = new Category {CategoryId=1, Name = "Pizza"};
            _fakeContext.Categories.Add(pizzaCategory);

            var _Margeritha = new Dish { DishId = 1, Name = "Margeritha", Price = 79, Category = pizzaCategory, CategoryId = pizzaCategory.CategoryId };
            var margerithaCheese = new DishIngredient { Ingredient = ingredientCheese, Dish = _Margeritha };
            var margerithaTomatoe = new DishIngredient { Ingredient = ingredientTomatoe, Dish = _Margeritha };
            _fakeContext.DishIngredients.Add(margerithaCheese);
            _fakeContext.DishIngredients.Add(margerithaTomatoe);

            _Margeritha.DishIngredients = new List<DishIngredient> { margerithaCheese, margerithaTomatoe };
            _fakeContext.Dishes.Add(_Margeritha);
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
            _fakeContext.Add(user);
            _fakeContext.SaveChanges();
        }

        [Fact]
        public void Test_Price_On_Dish()
        {
            var httpContext = new DefaultHttpContext();

            //httpContext.Session.SetString("Session", cartId);
            
            var cartService = _serviceProvider.GetService<ICartService>();

            var cart = cartService.NewCart(DishCreator()).Result;

            var price = cart.CartItem[0].Price;

            Assert.Equal(79, price);
        }

        [Fact]
        public void Test_Price_On_Dish__With_Extra_Ingredient()
        {
            var cartService = _serviceProvider.GetService<ICartService>();

            var viewContext = new ViewContext()
            {
                HttpContext = new DefaultHttpContext()
            };
            viewContext.HttpContext.Session = new TestSession();
            var dish = DishCreator();
            var ingredients = IngredientMaker();
            var cart = cartService.NewCart(dish).Result;
            viewContext.HttpContext.Session.SetInt32("Cart", cart.CartId);

            var cartItem = cart.CartItem.Find(x => x.DishId == 1);
            
            var vm = new EditCartViewModel
            {
                CartItem = cartItem,
                CartItemIngredients = cartItem.CartItemIngredient,
                AllIngredients = ingredients,
                CartItemId = cartItem.CartItemId
            };

            var model = cartService.EditCartItemIngredients(vm).Result;
            var dishPrice = dish.Price;
            var ingPrice = vm.AllIngredients.FindAll(x=>x.Checked = true).Sum(x => x.IngredientPrice);

            Assert.Equal(89, model.CartItem.Price);
        }

        public Dish DishCreator()
        {
            var ingredientTomatoe = new Ingredient { Name = "Tomatoe", IngredientPrice = 5 };
            var ingredientCheese = new Ingredient { Name = "Cheese", IngredientPrice = 5 };

            var pizzaCategory = new Category { CategoryId = 1, Name = "Pizza" };

            var margeritha = new Dish { DishId = 1, Name = "Margeritha", Price = 79, Category = pizzaCategory, CategoryId = pizzaCategory.CategoryId };
            var margerithaCheese = new DishIngredient { Ingredient = ingredientCheese, Dish = margeritha };
            var margerithaTomatoe = new DishIngredient { Ingredient = ingredientTomatoe, Dish = margeritha };

            margeritha.DishIngredients = new List<DishIngredient> { margerithaCheese, margerithaTomatoe };

            return margeritha;
        }

        public List<Ingredient> IngredientMaker()
        {
            var list = new List<Ingredient>();
            var ingredientTomatoe = new Ingredient { Name = "Tomatoe", IngredientPrice = 5, IngredientId = 1, Checked = true };
            var ingredientCheese = new Ingredient { Name = "Cheese", IngredientPrice = 5, IngredientId = 2, Checked = true };
            var ingredientShrimp = new Ingredient { Name = "Shrimp", IngredientPrice = 5, IngredientId = 3, Checked = false };
            list.Add(ingredientTomatoe);
            list.Add(ingredientCheese);
            list.Add(ingredientShrimp);
            return list;
        }
    }
}
