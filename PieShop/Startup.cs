using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using PieShop.Models;       // Adding for own services from the same project folder

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore; // for UseSqlServer()
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PieShop
{
    public class Startup
    {
        // Creating a connection string: entry point to data 
        private IConfigurationRoot _configruationRoot;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            // Creating a new Configuration Builder 
            _configruationRoot = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath) // Root 
                                                                 // .AddJsonFile("appsettings.json")     // Removing because we add appsettings.production.json

                // If the setting set to the production, it will automaticall triggered the appsettings.production.json file. 
                // Need to update the database inside the PackageManager Console
                // $ Update-Database -Context AppDbContext -Environment Production
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true)
                .Build(); 

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Registring the DB content 
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_configruationRoot.GetConnectionString("DefaultConnection")));

            // Adding for Identiy Framework 
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>(); 

            // Adding the own servercies from Models 
            // services.AddTransient<ICategoryRepository, MockCategoryRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>(); // Need to import the Models Folder

            // services.AddTransient<IPieRepository, MockPieRepository>();
            services.AddTransient<IPieRepository, PieRepository>();

            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp)); // Get the obj with request

            // Adding for Orders 
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddMvc(); // Adding the MVC 20-Jun-17 11:08 am

            // Session 
            services.AddMemoryCache();
            services.AddSession(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /*
             // Removing the default codes 
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
            */


            // Checking either development or production stage
            if (env.IsDevelopment ())
            {
                // Adding the middle ware components (20-Jun-17 11:04 am) -- IApplicationBuilder: MiddleWare Pipeline
                app.UseDeveloperExceptionPage();    // Showing the exepection on browser
                app.UseStatusCodePages();           // Supports text only header or commmon status -- handles HTTP header 400-600
            }
            else
            {
                app.UseExceptionHandler("/AppException"); 
            }
           
            app.UseStaticFiles();               // Enable to serve the static files such as JavaScript, HTML, etc...

            // Adding the middle ware for session. Note *** has to be before app.UseMvcWithDefaultRoute();
            app.UseSession();
            app.UseIdentity(); // Adding for security: UserName and Password check

            // app.UseMvcWithDefaultRoute();       // DefaultRoute middleware
            app.UseMvc (routes =>
            {
                // Adding the route for PieController
                routes.MapRoute(
                        name: "categoryfilter",
                        template: "Pie/{action}/{category?}",
                        defaults: new { Controller = "Pie", action = "List" }
                        );
                

                routes.MapRoute(
                            name: "default",
                            template: "{controller=Home}/{action=Index}/{id?}" 
                );
            }); 
            

            // Adding the Data to the Database 
            DbInitializer.Seed(app);            // Checking the initial data
        }
    }
}
