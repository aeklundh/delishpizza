using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaDelish.Data;
using PizzeriaDelish.Extensions;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Services
{
    public class CartService
    {
        private readonly WebshopDbContext _context;
        private readonly ISession _session;

        public CartService(WebshopDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }

        public List<CartItem> GetCart()
        {
            if (!_session.CartIsEmpty())
                return _session.DeserialiseCart();
            else
                return null;
        }

        public void EmptyCart()
        {
            _session.SetString("cart", String.Empty);
        }

        public void AddToCart(int dishId)
        {
            Dish dish = _context.Dishes
                .Include(x => x.DishIngredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefault(x => x.DishId == dishId);
            if (dish != null)
            {
                List<Ingredient> ingredients = new List<Ingredient>();
                foreach (DishIngredient di in dish.DishIngredients)
                {
                    ingredients.Add(di.Ingredient);
                }

                List<CartItem> cart;
                CartItem addItem = new CartItem(dish) {
                    CartItemId = Guid.NewGuid(),
                    Ingredients = ingredients
                };

                if (_session.CartIsEmpty())
                {
                    cart = new List<CartItem>() { addItem };
                }
                else
                {
                    cart = _session.DeserialiseCart();
                    cart.Add(addItem);
                }
                _session.SerialiseCart(cart);
            }
        }

        public void AlterItem(int ingredientId, bool add, Guid cartItemId)
        {
            Ingredient ingredient = _context.Ingredients.FirstOrDefault(x => x.IngredientId == ingredientId);

            if (ingredient != null && !_session.CartIsEmpty())
            {
                List<CartItem> cart = _session.DeserialiseCart();
                CartItem toAlter = cart.FirstOrDefault(x => x.CartItemId == cartItemId);
                if (toAlter != null)
                {
                    if (add)
                        toAlter.Ingredients.Add(ingredient);
                    else
                        toAlter.Ingredients.RemoveAll(x => x.IngredientId == ingredientId);

                    _session.SerialiseCart(cart);
                }
            }
        }

        public void RemoveItem(Guid cartItemId)
        {
            if (!_session.CartIsEmpty())
            {
                List<CartItem> cart = _session.DeserialiseCart();
                CartItem cartItem = cart.FirstOrDefault(x => x.CartItemId == cartItemId);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    _session.SerialiseCart(cart);
                }
            }
        }

        public int CalculatePrice(CartItem cartItem)
        {
            int sum = cartItem.Dish.Price;
            List<Ingredient> ingredients = new List<Ingredient>();
            Dish baseDish = _context.Dishes
                .Include(x => x.DishIngredients)
                .FirstOrDefault(x => x.DishId == cartItem.Dish.DishId);
            if (baseDish != null)
            {
                foreach (Ingredient i in cartItem.Ingredients)
                {
                    if (baseDish.DishIngredients.FirstOrDefault(x => x.IngredientId == i.IngredientId) == null)
                    {
                        Ingredient ingredient = _context.Ingredients.FirstOrDefault(x => x.IngredientId == i.IngredientId);
                        if (ingredient != null)
                        {
                            sum += ingredient.Price;
                        }
                    }
                }
            }

            return sum;
        }

        public int CalculateTotalCartPrice(List<CartItem> cart)
        {
            int sum = 0;
            foreach (CartItem cartItem in cart)
            {
                sum += CalculatePrice(cartItem);
            }
            return sum;
        }
    }
}
