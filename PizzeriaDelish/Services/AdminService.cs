using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Services
{
    public class AdminService
    {
        private readonly WebshopDbContext _context;

        public AdminService(WebshopDbContext context)
        {
            _context = context;
        }

        public async Task UpdateDishAsync(Dish dish, List<DishIngredient> dishIngredients = null)
        {
            _context.Update(dish);
            await _context.SaveChangesAsync();

            if (dishIngredients != null)
            {
                //remove old DishIngredients
                List<DishIngredient> oldDishIngredients = _context.DishIngredients.Where(di => di.DishId == dish.DishId).ToList();
                _context.DishIngredients.RemoveRange(oldDishIngredients);
                await _context.SaveChangesAsync();

                //add new DishIngredients
                _context.DishIngredients.AddRange(dishIngredients);
                await _context.SaveChangesAsync();
            }
        }
    }
}
