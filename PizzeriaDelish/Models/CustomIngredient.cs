using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class CustomIngredient
    {
        public int DishOrderId { get; set; }
        public int IngredientId { get; set; }
        public bool IsAdded { get; set; }

        public DishOrder DishOrder { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
