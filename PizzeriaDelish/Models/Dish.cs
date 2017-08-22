using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int TypeId { get; set; }

        public ICollection<DishIngredient> DishIngredients { get; set; }
        public ICollection<DishCategory> DishCategories { get; set; }
        public ICollection<DishOrder> DishOrders { get; set; }
    }
}
