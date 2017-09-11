using PizzeriaDelish.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PizzeriaDelish.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace PizzeriaDelishTests
{
    public class CartTests : BaseTest
    {
        private readonly CartService _cartService;

        public CartTests()
        {
            _cartService = new CartService(_context, _httpContextAccessor);
        }

        protected override void InitialiseDatabase()
        {
            base.InitialiseDatabase();

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

            _context.Dishes.AddRange(dishes);
            _context.Ingredients.AddRange(ingredients);
            _context.DishIngredients.AddRange(dishIngredients);
            _context.SaveChanges();
        }

        [Fact]
        public async void Calculate_CartItem()
        {
            //Assemble
            CartItem withAddedIngredient = await _cartService.CreateCartItemAsync(1);
            CartItem withoutAddedIngredient = await _cartService.CreateCartItemAsync(2);

            Ingredient ingredient = _context.Ingredients.Single(x => x.IngredientId == 2);
            withAddedIngredient.Ingredients.Add(ingredient);

            //Act
            int expectedWithout = withoutAddedIngredient.Dish.Price;
            int actualWithout = _cartService.CalculatePrice(withoutAddedIngredient);

            int expectedWith = withAddedIngredient.Dish.Price + ingredient.Price;
            int actualWith = _cartService.CalculatePrice(withAddedIngredient);

            //Assert
            Assert.Equal(expectedWithout, actualWithout);
            Assert.Equal(expectedWith, actualWith);
        }
    }
}
