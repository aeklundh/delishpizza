using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzeriaDelish.Extensions;

namespace PizzeriaDelish.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly WebshopDbContext _context;

        public CartViewComponent(WebshopDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CartItem> cart;
            if (HttpContext.Session.CartIsEmpty())
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = HttpContext.Session.DeserialiseCart();
            }

            return View("_Cart", cart);
        }
    }
}
