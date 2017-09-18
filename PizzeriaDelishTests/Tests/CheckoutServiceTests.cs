using Microsoft.EntityFrameworkCore;
using PizzeriaDelish.Models;
using PizzeriaDelish.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PizzeriaDelishTests.Tests
{
    public class CheckoutServiceTests : BaseTest
    {
        private readonly CheckoutService _checkoutService;
        private readonly CartService _cartService;
        private readonly AddressService _addressService;

        public CheckoutServiceTests()
        {
            _checkoutService = new CheckoutService(_context, _httpContextAccessor);
            _cartService = new CartService(_context, _httpContextAccessor);
            _addressService = new AddressService(_context);
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

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async void Create_Order(bool customisedItems)
        {
            //Assemble
            List<CartItem> cart = await CreateCart(customisedItems);
            Address address = await _addressService.AddAddressAsync(new Address() { City = "aaa", FirstName = "bbb", Surname = "ccc", PostalCode = "ddd", StreetAddress = "eee" });

            //Act
            await _checkoutService.CreateOrderAsync(cart, address, false);

            //Assert
            Order order = _context.Orders.Include(x => x.DishOrders).SingleOrDefault();
            Assert.NotNull(order);
            Assert.True(order.DishOrders.Count == 3);
        }

        private async Task<List<CartItem>> CreateCart(bool customisedItems)
        {
            List<CartItem> retVal = new List<CartItem>();
            for (int i = 1; i < 4; i++)
            {
                CartItem item = await _cartService.CreateCartItemAsync(i);
                retVal.Add(item);
                retVal.Add(item);
            }
            if (customisedItems)
            {
                retVal[0].Ingredients.Add(await _context.Ingredients.SingleAsync(x => x.IngredientId == 1));
                retVal[0].Ingredients.Add(await _context.Ingredients.SingleAsync(x => x.IngredientId == 2));
                retVal[2].Ingredients.RemoveAt(0);
            }
            return retVal;
        }
    }
}
