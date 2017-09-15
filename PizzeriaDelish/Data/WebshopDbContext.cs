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

            //set DishOrderIngredient keys
            builder.Entity<DishOrderIngredient>()
                .HasKey(di => new { di.DishOrderId, di.IngredientId });

            builder.Entity<DishOrderIngredient>()
                .HasOne(di => di.DishOrder)
                .WithMany(di => di.DishOrderIngredients)
                .HasForeignKey(di => di.DishOrderId);

            builder.Entity<DishOrderIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(di => di.DishOrderIngredients)
                .HasForeignKey(di => di.IngredientId);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DishOrderIngredient> DishOrderIngredients { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
