using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }

        public ICollection<DishIngredient> DishIngredients { get; set; }
    }
}
