﻿using Microsoft.AspNetCore.Identity;
using PizzeriaDelish.Models;
using PizzeriaDelish.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Data
{
    public static class DbInitialiser
    {
        public static void Initialise(WebshopDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AddressService addressService)
        {
            if (context.Roles.Count() == 0)
            {
                IdentityRole adminRole = new IdentityRole() { Name = "Admin" };
                roleManager.CreateAsync(adminRole).Wait();
            }

            if (context.Addresses.Count() == 0)
            {
                Address seededUserAddress = addressService.AddAddressAsync(new Address()
                {
                    StreetAddress = "Hemvägen 5",
                    City = "Huddinge",
                    PostalCode = "14140",
                    FirstName = "Ture",
                    Surname = "Testare"
                }).Result;

                Address adminUserAddress = addressService.AddAddressAsync(new Address()
                {
                    StreetAddress = "Bortgången 3",
                    City = "Molnet",
                    PostalCode = "14141",
                    FirstName = "Admin",
                    Surname = "Admin"
                }).Result;
            }

            if (context.Users.Count() == 0)
            {
                ApplicationUser seededUser = new ApplicationUser()
                {
                    UserName = "ture@test.net",
                    Email = "ture@test.net",
                    PhoneNumber = "0701234567",
                    AddressId = 1
                };
                ApplicationUser adminUser = new ApplicationUser()
                {
                    UserName = "admin@delish.se",
                    Email = "admin@delish.se",
                    PhoneNumber = "0707654321",
                    AddressId = 2
                };

                userManager.CreateAsync(seededUser, "Test12#").Wait();
                userManager.CreateAsync(adminUser, "PassWeCan2!").Wait();
                userManager.AddToRoleAsync(adminUser, "Admin").Wait();
            }

            if (context.Dishes.Count() == 0)
            {
                List<Category> categories = new List<Category>() {
                    new Category() { Name = "Pizza", Active = true },
                    new Category() { Name = "Sallad", Active = true },
                    new Category() { Name = "Tillbehör", Active = true },
                    new Category() { Name = "Kebab", Active = true },
                    new Category() { Name = "Övrigt", Active = true }
                };

                List<Ingredient> ingredients = new List<Ingredient>() {
                    new Ingredient() { Name = "Skinka", Price = 10 },
                    new Ingredient() { Name = "Ananas", Price = 10 },
                    new Ingredient() { Name = "Champinjoner", Price = 10 },
                    new Ingredient() { Name = "Oxfilé", Price = 15 }
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
            }

            context.SaveChanges();
        }
    }
}
