using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using Newtonsoft.Json;
using PizzeriaDelish.Extensions;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaDelish.Controllers
{
    public class CartController : Controller
    {
        private readonly WebshopDbContext _context;

        public CartController(WebshopDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public ActionResult GetCart()
        {
            return ViewComponent("Cart");
        }

        [HttpPost]
        public ActionResult AddToCart(int dishId)
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
                if (HttpContext.Session.CartIsEmpty())
                {
                    cart = new List<CartItem>() { addItem };
                }
                else
                {
                    cart = HttpContext.Session.DeserialiseCart();
                    cart.Add(addItem);
                }
                HttpContext.Session.SerialiseCart(cart);
            }

            return ViewComponent("Cart");
        }

        [HttpGet]
        public ActionResult AlterItem(Guid cartItemId)
        {
            return ViewComponent("CartItemDetails", cartItemId);
        }

        // POST: Cart/Edit/{cartItem}
        [HttpPost]
        public ActionResult AlterItem(int? ingredientId, bool add, Guid cartItemId)
        {
            Ingredient ingredient = _context.Ingredients.FirstOrDefault(x => x.IngredientId == ingredientId);

            if (ingredient != null && !HttpContext.Session.CartIsEmpty())
            {
                List<CartItem> cart = HttpContext.Session.DeserialiseCart();
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

                    HttpContext.Session.SerialiseCart(cart);
                }
            }
            return ViewComponent("CartItemDetails", cartItemId);
        }

        // GET: Cart/Delete/5
        [HttpPost]
        public ActionResult RemoveItem(Guid cartItemId)
        {
            if (!HttpContext.Session.CartIsEmpty())
            {
                List<CartItem> cart = HttpContext.Session.DeserialiseCart();
                CartItem cartItem = cart.FirstOrDefault(x => x.CartItemId == cartItemId);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    HttpContext.Session.SerialiseCart(cart);
                }
            }

            return ViewComponent("Cart");
        }
    }
}