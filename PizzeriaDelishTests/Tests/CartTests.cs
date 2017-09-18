using Moq;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using PizzeriaDelish.Services;
using PizzeriaDelishTests.Utilities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PizzeriaDelishTests.Tests
{
    public class CartTests
    {
        [Fact]
        public void Calculate_Price_Of_CartItem()
        {
            //Assemble
            //set up test data
            List<DishIngredient> dishIngredients = new List<DishIngredient>() {
                new DishIngredient() { DishId = 1, IngredientId = 1},
            };
            IQueryable<Dish> dishData = new List<Dish>() {
                new Dish() { DishId = 1, Price = 50, DishIngredients = dishIngredients  },
                new Dish() { DishId = 2, Price = 60, DishIngredients = new List<DishIngredient>() }
            }.AsQueryable();

            IQueryable<Ingredient> ingredientData = new List<Ingredient>() {
                new Ingredient() { IngredientId = 1, Price = 5 },
                new Ingredient() { IngredientId = 2, Price = 10 }
            }.AsQueryable();

            //set up database
            var mockDishSet = DbMockHelper.CreateMockDbSet(dishData);
            var mockIngredientSet = DbMockHelper.CreateMockDbSet(ingredientData);

            Mock<WebshopDbContext> dbContext = new Mock<WebshopDbContext>();
            dbContext.Setup(c => c.Dishes).Returns(mockDishSet.Object);
            dbContext.Setup(c => c.Ingredients).Returns(mockIngredientSet.Object);

            //create service
            CartService cartService = new CartService(dbContext.Object, InjectionMockHelper.CreateMockHttpAccessor().Object);

            //create cart
            CartItem unaltered = new CartItem(dishData.First()); //price: 50
            unaltered.Ingredients.Add(ingredientData.First(x => x.IngredientId == 1)); //+ 0

            CartItem hasHadRemoved = new CartItem(dishData.First(x => x.DishId == 1)); //price: 50

            CartItem hasHadAdded = new CartItem(dishData.First(x => x.DishId == 2)); //price: 60
            hasHadAdded.Ingredients.Add(ingredientData.First(x => x.IngredientId == 1)); //+5
            hasHadAdded.Ingredients.Add(ingredientData.First(x => x.IngredientId == 2)); //+10

            //Act
            int actual1 = cartService.CalculatePrice(unaltered);
            int actual2 = cartService.CalculatePrice(hasHadRemoved);
            int actual3 = cartService.CalculatePrice(hasHadAdded);

            //Assert
            Assert.Equal(50, actual1);
            Assert.Equal(50, actual2);
            Assert.Equal(75, actual3);
        }
    }
}
