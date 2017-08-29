using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzeriaDelish.Models;

namespace PizzeriaDelish.Data
{
    public class WebshopDbContext : IdentityDbContext<ApplicationUser>
    {
        public WebshopDbContext(DbContextOptions<WebshopDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //set DishIngredient keys
            builder.Entity<DishIngredient>()
                .HasKey(di => new { di.DishId, di.IngredientId });

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(di => di.DishIngredients)
                .HasForeignKey(di => di.DishId);

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(di => di.DishIngredients)
                .HasForeignKey(di => di.IngredientId);

            //set DishOrder keys
            builder.Entity<DishOrder>()
                .HasOne(dor => dor.Dish)
                .WithMany(dor => dor.DishOrders)
                .HasForeignKey(dor => dor.DishId);

            builder.Entity<DishOrder>()
                .HasOne(dor => dor.Order)
                .WithMany(dor => dor.DishOrders)
                .HasForeignKey(dor => dor.OrderId);

            //set customingredient key
            builder.Entity<CustomIngredient>()
                .HasKey(ci => new { ci.DishOrderId, ci.IngredientId });

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<DishCategory> DishCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CustomIngredient> CustomIngredients { get; set; }
    }
}
