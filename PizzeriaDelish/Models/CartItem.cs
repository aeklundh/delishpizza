using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class CartItem
    {
        public Dish Dish { get; set; }
        public List<CustomIngredient> CustomIngredients { get; set; }
    }
}
