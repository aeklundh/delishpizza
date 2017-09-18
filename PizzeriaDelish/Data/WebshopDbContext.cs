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

        public WebshopDbContext()
        { }

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

        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<DishIngredient> DishIngredients { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<DishOrderIngredient> DishOrderIngredients { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
    }
}
