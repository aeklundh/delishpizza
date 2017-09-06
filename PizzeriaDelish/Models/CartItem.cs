using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class CartItem
    {
        public Guid CartItemId { get; set; }
        public Dish Dish { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public CartItem(Dish dish)
        {
            Dish = dish;
        }
    }
}
