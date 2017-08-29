using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models.DishViewModels
{
    public class EditViewModel
    {
        public Dish Dish { get; set; }
        //public List<CheckedIngredient> Ingredients { get; set; } = new List<CheckedIngredient>();
        public List<SelectListItem> Ingredients { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Categories { get; set; }

        public EditViewModel(Dish dish, List<Category> categories, List<Ingredient> ingredients)
        {
            Dish = dish;
            List<SelectListItem> selectCats = new List<SelectListItem>();
            foreach (Category cat in categories)
            {
                selectCats.Add(new SelectListItem() { Text = cat.Name, Value = cat.CategoryId.ToString() });
            }
            Categories = selectCats;

            foreach (Ingredient i in ingredients)
            {
                Ingredients.Add(new SelectListItem() {
                    Text = i.Name,
                    Value = i.IngredientId.ToString(),
                    Selected = (dish.DishIngredients.FirstOrDefault(x => x.IngredientId == i.IngredientId) != null)
                    });
            }

            //foreach (Ingredient i in ingredients)
            //{
            //    if (dish.DishIngredients.FirstOrDefault(x => x.IngredientId == i.IngredientId) == null)
            //        Ingredients.Add(new CheckedIngredient() { Ingredient = i, Checked = false });
            //    else
            //        Ingredients.Add(new CheckedIngredient() { Ingredient = i, Checked = true });
            //}
        }

        public EditViewModel()
        {

        }

        public class CheckedIngredient
        {
            public bool Checked { get; set; }
            public Ingredient Ingredient { get; set; }
        }
    }
}
