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

        public ActionResult AddToCart(int dishId)
        {
            Dish dish = _context.Dishes.FirstOrDefault(x => x.DishId == dishId);
            if (dish != null)
            {
                List<CartItem> cart;
                if (HttpContext.Session.CartIsEmpty())
                {
                    if (dish != null)
                    {
                        cart = new List<CartItem>() { new CartItem(dish) };
                        HttpContext.Session.SerialiseCart(cart);
                    }
                }
                else
                {
                    cart = HttpContext.Session.DeserialiseCart();
                    cart.Add(new CartItem(dish));
                }
            }

            return ViewComponent("Cart");
        }

        // GET: Cart/Edit
        public ActionResult EditDish(Dish dish)
        {
            return View();
        }

        // POST: Cart/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveDish(CartItem cartItem)
        {
            if (!HttpContext.Session.CartIsEmpty())
            {
                List<CartItem> cart = HttpContext.Session.DeserialiseCart();
                CartItem toRemove = cart.FirstOrDefault(x => x == cartItem);
                if (toRemove != null)
                {
                    cart.Remove(toRemove);
                }
                HttpContext.Session.SerialiseCart(cart);
            }
            return ViewComponent("Cart");
        }
    }
}