using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class DishOrder
    {
        public int DishOrderId { get; set; }
        public int DishId { get; set; }
        public int OrderId { get; set; }
        [Required]
        public int Amount { get; set; }

        public Dish Dish { get; set; }
        public Order Order { get; set; }
        public List<DishOrderIngredient> DishOrderIngredients { get; set; }
    }
}
