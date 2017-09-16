using Microsoft.EntityFrameworkCore;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Services
{
    public class IngredientService
    {
        private readonly WebshopDbContext _context;

        public IngredientService(WebshopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ingredient>> GetIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }
    }
}
