using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using Microsoft.AspNetCore.Authorization;
using PizzeriaDelish.Models.DishViewModels;

namespace PizzeriaDelish.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DishController : Controller
    {
        private readonly WebshopDbContext _context;

        public DishController(WebshopDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            IndexViewModel vm = new IndexViewModel(await _context.Dishes.ToListAsync(), await _context.Categories.ToListAsync());
            return View(vm);
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(x => x.Category)
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View(new CreateViewModel(_context.Categories.ToList()));
        }

        // POST: Dishes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,Name,Description,Price,CategoryId,Active")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient).SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(new EditViewModel(dish, await _context.Categories.ToListAsync(), await _context.Ingredients.ToListAsync()));
        }

        // POST: Dishes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditViewModel vm) //int id, [Bind("DishId,Name,Description,Price,CategoryId,Active")] Dish dish, List<Ingredient> selectedIngredients
        {
            if (id != vm.Dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm.Dish);
                    await _context.SaveChangesAsync();

                    //remove old DishIngredients
                    List<DishIngredient> dishIngredients = _context.DishIngredients.Where(di => di.DishId == vm.Dish.DishId).ToList();
                    _context.DishIngredients.RemoveRange(dishIngredients);
                    await _context.SaveChangesAsync();

                    //create and add new DishIngredients
                    dishIngredients = new List<DishIngredient>();
                    foreach (SelectListItem ingredient in vm.Ingredients)
                    {
                        if (ingredient.Selected)
                        {
                            Ingredient i = _context.Ingredients.FirstOrDefault(x => x.IngredientId == int.Parse(ingredient.Value));
                            if (i != null)
                                dishIngredients.Add(new DishIngredient() { DishId = vm.Dish.DishId, IngredientId = i.IngredientId });
                        }
                    }
                    _context.DishIngredients.AddRange(dishIngredients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(vm.Dish.DishId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
