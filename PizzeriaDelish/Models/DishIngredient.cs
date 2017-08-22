using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class DishIngredient
    {
        public int DishId { get; set; }
        public int IngredientId { get; set; }

        public Dish Dish { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
