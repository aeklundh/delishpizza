using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaDelish.Data;
using PizzeriaDelish.Extensions;
using PizzeriaDelish.Models;
using PizzeriaDelish.Models.ViewComponentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.ViewComponents
{
    public class CartItemDetailsViewComponent : ViewComponent
    {
        private readonly WebshopDbContext _context;

        public CartItemDetailsViewComponent(WebshopDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid cartItemId)
        {
            CartItemDetailsViewComponentViewModel vm;
            CartItem cartItem = HttpContext.Session.DeserialiseCart().FirstOrDefault(x => x.CartItemId == cartItemId);
            if (cartItem != null)
            {
                List<Ingredient> availableIngredients = _context.Ingredients.ToList();
                List<Ingredient> addedIngredients = new List<Ingredient>();
                foreach (CustomIngredient ingredient in cartItem.CustomIngredients)
                {
                    availableIngredients.RemoveAll(x => x.IngredientId == ingredient.IngredientId);
                    addedIngredients.Add(_context.Ingredients.FirstOrDefault(x => x.IngredientId == ingredient.IngredientId));
                }
                vm = new CartItemDetailsViewComponentViewModel(cartItem, availableIngredients, addedIngredients);
                return View("_CartItemDetails", vm);
            }
            else
            {
                throw new Exception("404 - Not found");
            }
        }
    }
}
