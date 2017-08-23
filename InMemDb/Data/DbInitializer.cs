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
            var adminUserResult = userManager.CreateAsync(adminUser, "Javisst7/").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if(context.Dishes.ToList().Count == 0)
            {
                var capricciosa = new Dish { Name = "Capricciosa", Price=79 };
                var margaritha = new Dish { Name = "Margaritha", Price = 69 };
                var hawaii = new Dish { Name = "Hawaii", Price = 85 };
                context.AddRange(capricciosa, margaritha, hawaii);
                context.SaveChanges();
            }

        }
    }
}
