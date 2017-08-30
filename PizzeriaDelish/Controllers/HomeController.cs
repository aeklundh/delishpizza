using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzeriaDelish.Models;
using PizzeriaDelish.Data;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaDelish.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebshopDbContext _context;

        public HomeController(WebshopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Dishes.Include(x => x.Category).ToList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
