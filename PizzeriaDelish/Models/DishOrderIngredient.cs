using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class DishOrderIngredient
    {
        public int DishOrderId { get; set; }
        public int IngredientId { get; set; }
        [Required]
        public bool IsAdded { get; set; }

        public DishOrder DishOrder { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
