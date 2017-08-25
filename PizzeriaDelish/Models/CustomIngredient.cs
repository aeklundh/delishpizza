using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class CustomIngredient
    {
        public int DishOrderId { get; set; }
        public int IngredientId { get; set; }
        [Required]
        public bool IsAdded { get; set; }

        public CustomIngredient(int ingredientId, bool isAdded)
        {
            IngredientId = ingredientId;
            IsAdded = isAdded;
        }

        public DishOrder DishOrder { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
