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
            if (context.Users.ToList().Count == 0)
            { 
                var aUser = new ApplicationUser();
            aUser.UserName = "student@test.com";
            aUser.Email = "student@test.com";
            aUser.City = "Nacka";
            aUser.Firstname = "Erik";
            aUser.Lastname = "Jonson";
            aUser.ZipCode = "13136";
            aUser.Street = "Gatan";
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";
            adminUser.City = "Nacka";
            adminUser.Firstname = "Erik";
            adminUser.Lastname = "Jonson";
            adminUser.ZipCode = "13136";
            adminUser.Street = "Gatan";
            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            userManager.AddToRoleAsync(adminUser, "Admin").Wait();
            }
            if (context.Dishes.ToList().Count == 0)
            {
                var cheese = new Ingredient { Name = "Cheese", IngredientPrice=5 };
                var tomatoe = new Ingredient { Name = "Tomatoe", IngredientPrice = 5 };
                var ham = new Ingredient { Name = "Ham", IngredientPrice = 5 };
                var mozzarella = new Ingredient { Name = "Mozzarella", IngredientPrice = 5 };
                var gorgonzola = new Ingredient { Name = "Gorgonzola", IngredientPrice = 5 };
                var pineapple = new Ingredient { Name = "Pineapple", IngredientPrice = 5 };
                var lettuce = new Ingredient { Name = "Lettuce", IngredientPrice = 5 };
                var olives = new Ingredient { Name = "Olives", IngredientPrice = 5 };
                var chicken = new Ingredient { Name = "Chicken", IngredientPrice = 15 };
                var feta = new Ingredient { Name = "Feta", IngredientPrice = 5 };
                var ruccola = new Ingredient { Name = "Ruccola", IngredientPrice = 5 };
                var parmigiano = new Ingredient { Name = "Parmigiano", IngredientPrice = 5 };
                var tuna = new Ingredient { Name = "Tuna", IngredientPrice = 10 };
                var meat = new Ingredient { Name = "Meat", IngredientPrice = 15 };
                var cream = new Ingredient { Name = "Cream", IngredientPrice = 5 };
                var onion = new Ingredient { Name = "Onion", IngredientPrice = 5 };
                var bacon = new Ingredient { Name = "Bacon", IngredientPrice = 10 };

                var categoryPizza = new Category { Name = "Pizza" };
                var categorySalad = new Category { Name = "Salad" };
                var categoryPasta = new Category { Name = "Pasta" };

                var carbonara = new Dish { Name = "Carbonara", Price = 89, Category = categoryPasta };
                var lasagne = new Dish { Name = "Lasagne", Price = 95, Category = categoryPasta };
                var pastaConTono = new Dish { Name = "Pasta Con Tono", Price = 92, Category = categoryPasta };
                var capricciosa = new Dish { Name = "Capricciosa", Price = 79, Category = categoryPizza };
                var margaritha = new Dish { Name = "Margaritha", Price = 69, Category = categoryPizza };
                var hawaii = new Dish { Name = "Hawaii", Price = 85, Category = categoryPizza };
                var quattroFormaggi = new Dish { Name = "Quattro Formaggio", Price = 95, Category = categoryPizza };
                var greekSalad = new Dish { Name = "Greek Salad", Price = 89, Category = categorySalad };
                var dietSalad = new Dish { Name = "Slim Salad", Price = 29, Category = categorySalad };

                var carbonaraCheese = new DishIngredient { Dish = carbonara, Ingredient = tomatoe };
                var carbonaraCream = new DishIngredient { Dish = carbonara, Ingredient = cream };
                var carbonaraBacon = new DishIngredient { Dish = carbonara, Ingredient = bacon };
                var lasagneCheese = new DishIngredient { Dish = lasagne, Ingredient = cheese };
                var lasagneCream = new DishIngredient { Dish = lasagne, Ingredient = cream };
                var lasagneMeat = new DishIngredient { Dish = lasagne, Ingredient = meat };
                var pastaConTonoTomatoSauce = new DishIngredient { Dish = pastaConTono, Ingredient = tomatoe };
                var pastaConTonoTuna = new DishIngredient { Dish = pastaConTono, Ingredient = tuna };
                var pastaConTonoOnion = new DishIngredient { Dish = pastaConTono, Ingredient = onion };

                var dietSaladRuccola = new DishIngredient { Dish = dietSalad, Ingredient = ruccola };
                var dietSaladParmigiano = new DishIngredient { Dish = dietSalad, Ingredient = parmigiano };
                dietSalad.DishIngredients = new List<DishIngredient>();
                dietSalad.DishIngredients.Add(dietSaladRuccola);
                dietSalad.DishIngredients.Add(dietSaladParmigiano);

                context.DishIngredients.AddRange(carbonaraCheese, carbonaraCream, carbonaraBacon,
                    lasagneCheese, lasagneCream, lasagneMeat,
                    pastaConTonoTuna, pastaConTonoTomatoSauce, pastaConTonoOnion);
                
                var greekSaladLettuce = new DishIngredient { Dish = greekSalad, Ingredient = lettuce };
                var greekSaladOlives = new DishIngredient { Dish = greekSalad, Ingredient = olives };
                var greekSaladChicken = new DishIngredient { Dish = greekSalad, Ingredient = chicken };
                var greekSaladFeta = new DishIngredient { Dish = greekSalad, Ingredient = feta };
                greekSalad.DishIngredients = new List<DishIngredient>();
                greekSalad.DishIngredients.Add(greekSaladLettuce);
                greekSalad.DishIngredients.Add(greekSaladOlives);
                greekSalad.DishIngredients.Add(greekSaladChicken);
                greekSalad.DishIngredients.Add(greekSaladFeta);

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
                context.Dishes.Add(greekSalad);
                context.Dishes.Add(dietSalad);
                context.Dishes.Add(carbonara);
                context.Dishes.Add(lasagne);
                context.Dishes.Add(pastaConTono);

                context.SaveChanges();
            }
        }
    }
}
