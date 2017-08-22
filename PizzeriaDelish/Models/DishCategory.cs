using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class DishCategory
    {
        public int DishId { get; set; }
        public int CategoryId { get; set; }

        public Dish Dish { get; set; }
        public Category Category { get; set; }
    }
}
