using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaDelish.Data;
using PizzeriaDelish.Models;
using PizzeriaDelish.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PizzeriaDelish
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _hostingEnvironment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_hostingEnvironment.IsProduction()
                || _hostingEnvironment.IsStaging()
                || _hostingEnvironment.EnvironmentName == "IntegratedDb"
                || _hostingEnvironment.EnvironmentName == "LocalSql")
            {
                services.AddDbContext<WebshopDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                services.AddDbContext<WebshopDbContext>(options =>
                    options.UseInMemoryDatabase("database"));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<WebshopDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<UserManager<ApplicationUser>>();
            services.AddTransient<RoleManager<IdentityRole>>();
            services.AddTransient<AddressService>();
            services.AddTransient<AdminService>();
            services.AddTransient<CartService>();
            services.AddTransient<CheckoutService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WebshopDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AddressService addressService, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddAzureWebAppDiagnostics();
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            if (_hostingEnvironment.IsProduction()
                || _hostingEnvironment.IsStaging()
                || _hostingEnvironment.EnvironmentName == "IntegratedDb"
                || _hostingEnvironment.EnvironmentName == "LocalSql")
            {
                context.Database.Migrate();
            }

            DbInitialiser.Initialise(context, userManager, roleManager, addressService);
        }
    }
}
