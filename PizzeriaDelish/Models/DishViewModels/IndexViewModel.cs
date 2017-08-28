using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models.DishViewModels
{
    public class IndexViewModel
    {
        public List<Dish> Dishes { get; set; }
        public List<Category> Categories { get; set; }

        public IndexViewModel(List<Dish> dishes, List<Category> categories)
        {
            Dishes = dishes;
            Categories = categories;
        }
    }
}
