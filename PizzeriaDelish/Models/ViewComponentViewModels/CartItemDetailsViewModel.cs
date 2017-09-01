using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models.ViewComponentViewModels
{
    public class CartItemDetailsViewComponentViewModel
    {
        public CartItem CartItem { get; set; }
        public List<Ingredient> AvailableIngredients { get; set; }
        public List<Ingredient> AddedIngredients { get; set; }

        public CartItemDetailsViewComponentViewModel(CartItem cartItem, List<Ingredient> availableIngredients, List<Ingredient> addedIngredients)
        {
            CartItem = cartItem;
            AvailableIngredients = availableIngredients;
            AddedIngredients = addedIngredients;
        }
    }
}
