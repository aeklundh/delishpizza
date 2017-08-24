using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<CartItem> cart = new List<CartItem>();
            string serialised = HttpContext.Session.GetString("cart");
            if (!String.IsNullOrWhiteSpace(serialised))
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(serialised);
            }

            return View("_Cart", cart);
        }
    }
}
