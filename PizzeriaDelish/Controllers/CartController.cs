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
using PizzeriaDelish.Services;

namespace PizzeriaDelish.Controllers
{
    public class CartController : Controller
    {
        private readonly WebshopDbContext _context;
        private readonly CartService _cartService;

        public CartController(WebshopDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: Cart
        public ActionResult GetCart()
        {
            return ViewComponent("Cart");
        }

        [HttpPost]
        public ActionResult AddToCart(int dishId)
        {
            _cartService.AddToCart(dishId);

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
            if (ingredientId != null)
            {
                _cartService.AlterItem((int)ingredientId, add, cartItemId);
            }

            return ViewComponent("CartItemDetails", cartItemId);
        }

        // GET: Cart/Delete/5
        [HttpPost]
        public ActionResult RemoveItem(Guid cartItemId)
        {
            _cartService.RemoveItem(cartItemId);

            return ViewComponent("Cart");
        }
    }
}