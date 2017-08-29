using Microsoft.AspNetCore.Identity;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Data
{
    public static class DbInitialiser
    {
        public static async void Initialise(WebshopDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            IdentityRole adminRole = new IdentityRole() { Name = "Admin" };
            await roleManager.CreateAsync(adminRole);

            ApplicationUser seededUser = new ApplicationUser()
            {
                UserName = "ture@test.net",
                Email = "ture@test.net",
                Address = "Hemvägen 5",
                City = "Huddinge",
                PostalCode = "14140",
                FirstName = "Ture",
                Surname = "Testare"
            };
            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin@delish.se",
                Email = "admin@delish.se",
                Address = "Bortgången 3",
                City = "Molnet",
                PostalCode = "14141",
                FirstName = "Admin",
                Surname = "Admin"
            };
            await userManager.CreateAsync(seededUser, "Test12#");
            await userManager.CreateAsync(adminUser, "PassWeCan2!");
            await userManager.AddToRoleAsync(adminUser, "Admin");

            List<Category> categories = new List<Category>() {
                new Category() { Name = "Pizza" },
                new Category() { Name = "Sallad" },
                new Category() { Name = "Tillbehör" },
                new Category() { Name = "Kebab" },
                new Category() { Name = "Pasta" },
                new Category() { Name = "Övrigt" }
            };
            List<Ingredient> ingredients = new List<Ingredient>() {
                new Ingredient() { Name = "Skinka" },
                new Ingredient() { Name = "Ananas" },
                new Ingredient() { Name = "Champinjoner" }
            };
            List<Dish> dishes = new List<Dish>() {
                new Dish() { Name = "Margherita", Price = 58, Description = "Den grundläggande pizzan, utan extra pålägg.", CategoryId = 1 },
                new Dish() { Name = "Vesuvio", Price = 58, Description = "Med skinka på.", CategoryId = 1 },
                new Dish() { Name = "Hawaii", Price = 58, Description = "Med ananas och skinka.", CategoryId = 1 },
                new Dish() { Name = "Capricciosa", Price = 58, Description = "En klassiker med champinjoner och skinka.", CategoryId = 1 }
            };
            List<DishIngredient> dishIngredients = new List<DishIngredient>() {
                new DishIngredient() { Dish = dishes[1], Ingredient = ingredients[0] },
                new DishIngredient() { Dish = dishes[2], Ingredient = ingredients[0] },
                new DishIngredient() { Dish = dishes[2], Ingredient = ingredients[1] },
                new DishIngredient() { Dish = dishes[3], Ingredient = ingredients[0] },
                new DishIngredient() { Dish = dishes[3], Ingredient = ingredients[2] }
            };

            context.Categories.AddRange(categories);
            context.Ingredients.AddRange(ingredients);
            context.Dishes.AddRange(dishes);
            context.DishIngredients.AddRange(dishIngredients);

            context.SaveChanges();

        }
    }
}
