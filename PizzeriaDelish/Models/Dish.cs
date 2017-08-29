using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public bool Active { get; set; }

        public ICollection<DishIngredient> DishIngredients { get; set; }
        public Category Category { get; set; }
        public ICollection<DishOrder> DishOrders { get; set; }
    }
}
