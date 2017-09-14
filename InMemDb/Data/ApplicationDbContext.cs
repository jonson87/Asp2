using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InMemDb.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InMemDb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DishIngredient>()
                .HasKey(di => new { di.DishId, di.IngredientId });

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.DishId);

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(i => i.DishIngredients)
                .HasForeignKey(di => di.IngredientId);

            builder.Entity<Dish>()
                .HasOne(o => o.Category)
                .WithMany(u => u.Dishes)
                .HasForeignKey(o => o.CategoryId);

            builder.Entity<CartItemIngredient>()
                 .HasOne(i => i.CartItem)
                 .WithMany(d => d.CartItemIngredient)
                 .HasForeignKey(x => x.CartItemId);

            builder.Entity<CartItemIngredient>()
                .HasOne(i => i.Ingredient)
                .WithMany(d => d.CartItemIngredient)
                .HasForeignKey(x => x.IngredientId);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemIngredient> CartItemIngredients { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
