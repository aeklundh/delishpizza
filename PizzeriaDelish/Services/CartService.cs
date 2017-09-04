﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public CartService(WebshopDbContext context)
        {
            _context = context;
        }

        public List<CartItem> GetCart(ISession session)
        {
            if (!session.CartIsEmpty())
                return session.DeserialiseCart();
            else
                return null;
        }

        public void AddToCart(ISession session, int dishId)
        {
            Dish dish = _context.Dishes
                .Include(x => x.DishIngredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefault(x => x.DishId == dishId);
            if (dish != null)
            {
                List<CartItem> cart;
                CartItem addItem = new CartItem(dish) { CartItemId = Guid.NewGuid() };
                List<CustomIngredient> addedIngredients = new List<CustomIngredient>();
                foreach (DishIngredient dishIngredient in dish.DishIngredients)
                {
                    addedIngredients.Add(new CustomIngredient(dishIngredient.IngredientId, true));
                }
                addItem.CustomIngredients.AddRange(addedIngredients);
                if (session.CartIsEmpty())
                {
                    cart = new List<CartItem>() { addItem };
                }
                else
                {
                    cart = session.DeserialiseCart();
                    cart.Add(addItem);
                }
                session.SerialiseCart(cart);
            }
        }

        public void AlterItem(ISession session, int ingredientId, bool add, Guid cartItemId)
        {
            Ingredient ingredient = _context.Ingredients.FirstOrDefault(x => x.IngredientId == ingredientId);

            if (ingredient != null && !session.CartIsEmpty())
            {
                List<CartItem> cart = session.DeserialiseCart();
                CartItem toAlter = cart.FirstOrDefault(x => x.CartItemId == cartItemId);
                if (toAlter != null)
                {
                    if (add)
                    {
                        toAlter.CustomIngredients.Add(new CustomIngredient(ingredient.IngredientId, true));
                    }
                    else
                    {
                        CustomIngredient customIngredient = toAlter.CustomIngredients.FirstOrDefault(x => x.IngredientId == ingredientId);
                        if (customIngredient != null)
                        {
                            toAlter.CustomIngredients.Remove(customIngredient);
                        }
                    }

                    session.SerialiseCart(cart);
                }
            }
        }

        public void RemoveItem(ISession session, Guid cartItemId)
        {
            if (!session.CartIsEmpty())
            {
                List<CartItem> cart = session.DeserialiseCart();
                CartItem cartItem = cart.FirstOrDefault(x => x.CartItemId == cartItemId);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    session.SerialiseCart(cart);
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
                foreach (CustomIngredient ci in cartItem.CustomIngredients.Where(x => x.IsAdded))
                {
                    if (baseDish.DishIngredients.FirstOrDefault(x => x.IngredientId == ci.IngredientId) == null)
                    {
                        Ingredient ingredient = _context.Ingredients.FirstOrDefault(x => x.IngredientId == ci.IngredientId);
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
