using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaDelish.Data;
using PizzeriaDelish.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzeriaDelishTests
{
    public abstract class BaseTest
    {
        protected readonly IServiceProvider _serviceProvider;

        protected BaseTest()
        {
            ServiceProvider efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<WebshopDbContext>(b =>
                b.UseInMemoryDatabase("dejtabaes")
                .UseInternalServiceProvider(efServiceProvider));

            services.AddTransient<AddressService>();
            services.AddTransient<AdminService>();
            services.AddTransient<CartService>();
            services.AddTransient<CheckoutService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _serviceProvider = services.BuildServiceProvider();

            InitialiseDatabase();
        }

        protected virtual void InitialiseDatabase()
        { }
    }
}
