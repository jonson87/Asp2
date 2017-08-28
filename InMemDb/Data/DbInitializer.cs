using InMemDb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Data
{
    public static class DbInitializer
    {

        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser();
            aUser.UserName = "student@test.com";
            aUser.Email = "student@test.com";
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";
            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if (context.Dishes.ToList().Count == 0)
            {
                var cheese = new Ingredient { Name = "Cheese" };
                var tomatoe = new Ingredient { Name = "Tomatoe" };
                var ham = new Ingredient { Name = "Ham" };
                var mozzarella = new Ingredient { Name = "Mozzarella" };
                var parmigiano = new Ingredient { Name = "Parmigiano" };
                var gorgonzola = new Ingredient { Name = "Gorgonzola" };
                var pineapple = new Ingredient { Name = "Pineapple" };

                var capricciosa = new Dish { Name = "Capricciosa", Price = 79 };
                var margaritha = new Dish { Name = "Margaritha", Price = 69 };
                var hawaii = new Dish { Name = "Hawaii", Price = 85 };
                var quattroFormaggi = new Dish { Name = "Quattro Formaggio", Price = 95 };

                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese };
                var capricciosaTomatoe = new DishIngredient { Dish = capricciosa, Ingredient = tomatoe };
                var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham };
                capricciosa.DishIngredients = new List<DishIngredient>();
                capricciosa.DishIngredients.Add(capricciosaTomatoe);
                capricciosa.DishIngredients.Add(capricciosaCheese);
                capricciosa.DishIngredients.Add(capricciosaHam);

                var margarithaTomatoe = new DishIngredient { Dish = margaritha, Ingredient = tomatoe };
                var margarithaCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };
                margaritha.DishIngredients = new List<DishIngredient>();
                margaritha.DishIngredients.Add(margarithaTomatoe);
                margaritha.DishIngredients.Add(margarithaCheese);

                var hawaiiTomatoe = new DishIngredient { Dish = hawaii, Ingredient = tomatoe };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };
                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = cheese };
                hawaii.DishIngredients = new List<DishIngredient>();
                hawaii.DishIngredients.Add(hawaiiTomatoe);
                hawaii.DishIngredients.Add(hawaiiPineapple);
                hawaii.DishIngredients.Add(hawaiiCheese);

                var quattroFormaggiCheese = new DishIngredient { Dish = quattroFormaggi, Ingredient = cheese };
                var quattroFormaggiMozzarella = new DishIngredient { Dish = quattroFormaggi, Ingredient = mozzarella };
                var quattroFormaggiGorgonzola = new DishIngredient { Dish = quattroFormaggi, Ingredient = gorgonzola };
                var quattroFormaggiParmigiano = new DishIngredient { Dish = quattroFormaggi, Ingredient = parmigiano };
                quattroFormaggi.DishIngredients = new List<DishIngredient>();
                quattroFormaggi.DishIngredients.Add(quattroFormaggiCheese);
                quattroFormaggi.DishIngredients.Add(quattroFormaggiMozzarella);
                quattroFormaggi.DishIngredients.Add(quattroFormaggiGorgonzola);
                quattroFormaggi.DishIngredients.Add(quattroFormaggiParmigiano);

                context.Dishes.Add(capricciosa);
                context.Dishes.Add(margaritha);
                context.Dishes.Add(quattroFormaggi);
                context.Dishes.Add(hawaii);
                context.SaveChanges();
            }

        }
    }
}
