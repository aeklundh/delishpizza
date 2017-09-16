using Microsoft.EntityFrameworkCore;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Services
{
    public class CategoryService
    {
        private readonly WebshopDbContext _context;

        public CategoryService(WebshopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
