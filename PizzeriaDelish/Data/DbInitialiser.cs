using Microsoft.AspNetCore.Identity;
using PizzeriaDelish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Data
{
    public static class DbInitialiser
    {
        public static void Initialise(WebshopDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            
        }
    }
}
