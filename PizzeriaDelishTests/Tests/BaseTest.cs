using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaDelish.Data;
using PizzeriaDelish.Services;
using PizzeriaDelishTests.FakeClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzeriaDelishTests.Tests
{
    public abstract class BaseTest
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly HttpContextAccessor _httpContextAccessor;
        protected WebshopDbContext _context { get; set; }
        
        protected BaseTest()
        {
            ServiceProvider efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<WebshopDbContext>(b =>
                b.UseInMemoryDatabase("database")
                .UseInternalServiceProvider(efServiceProvider));

            services.AddTransient<AddressService>();
            services.AddTransient<AdminService>();

            _serviceProvider = services.BuildServiceProvider();

            _httpContextAccessor = new HttpContextAccessor() { HttpContext = new DefaultHttpContext() };
            _httpContextAccessor.HttpContext.Session = new TestSession();

            InitialiseDatabase();
        }

        protected virtual void InitialiseDatabase() => _context = _serviceProvider.GetService<WebshopDbContext>();
    }
}
