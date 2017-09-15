using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaDelish.Data
{
    public class WebshopDbContextFactory : IDesignTimeDbContextFactory<WebshopDbContext>
    {
        public WebshopDbContext CreateDbContext(string[] args)
        {
            string conn = "Data Source=NIBNARD\\SQLEXPRESS;Initial Catalog=PizzeriaDelish;User Id=albin;Password=lösenord;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var optionsBuilder = new DbContextOptionsBuilder<WebshopDbContext>();
            optionsBuilder.UseSqlServer(conn);

            return new WebshopDbContext(optionsBuilder.Options);
        }
    }
}
