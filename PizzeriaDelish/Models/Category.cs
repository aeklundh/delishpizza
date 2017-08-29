using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }

        public ICollection<Dish> Dishes { get; set; }
    }
}
