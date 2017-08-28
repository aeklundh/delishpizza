﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Models.DishViewModels
{
    public class EditViewModel
    {
        public Dish Dish { get; set; }
        public List<SelectListItem> Categories { get; set; }

        public EditViewModel(Dish dish, List<Category> categories)
        {
            Dish = dish;
            List<SelectListItem> selectCats = new List<SelectListItem>();
            foreach (Category cat in categories)
            {
                selectCats.Add(new SelectListItem() { Text = cat.Name, Value = cat.CategoryId.ToString() });
            }
            Categories = selectCats;
        }
    }
}
