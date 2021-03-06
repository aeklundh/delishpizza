﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }

        public ICollection<DishIngredient> DishIngredients { get; set; }
        public ICollection<DishOrderIngredient> DishOrderIngredients { get; set; }
    }
}
