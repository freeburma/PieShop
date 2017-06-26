using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PieShop.Models;       // Adding the own repository
using PieShop.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        // Dependency injection
        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            this._pieRepository = pieRepository;
            this._categoryRepository = categoryRepository; 
        }

        // Creating an first action method return the list of views
        public ViewResult List(string category)
        {
            // Throwing to exception 
            // throw new Exception("Error");

            /*
            // For testing for List.cshtml 
            // Line 2:  @model IEnumerable<PieShop.Models.Pie>
            // Adding the extra properties instantly
            // ViewBag.CurrentCategory = "Donat"; // <!-- Displaying the instant in List.cshtml file < h1 > @ViewBag.CurrentCategory </ h1 > -->


            // return View(_pieRepository.Pies); // Returning hardcoded data
            */

            /*
            // This is a hardcoded controller
            PiesListViewModel pieListViewModel = new PiesListViewModel();
            pieListViewModel.Pies = _pieRepository.Pies;

            pieListViewModel.CurrentCategory = "Donat";
            return View(pieListViewModel);
            */

            IEnumerable<Pie> pies;
            string currentCategory = string.Empty; 

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.Pies.OrderBy(p => p.PieId);
                currentCategory = "All Pies";
            }
            else
            {
                pies = _pieRepository.Pies.Where(p => p.Category.CategoryName == category).OrderBy(p => p.PieId);
                currentCategory = _categoryRepository.Categories.FirstOrDefault(c => c.CategoryName == category).CategoryName; 
            }

            /*
                Added route in Startup.cs
                routes.MapRoute(
                        name: "categoryfilter",
                        template: "Pie/{action}/{category?}",
                        defaults: new { Controllers = "Pie", action = "List" }
                        );
             */
            return View(new PiesListViewModel
            {
                Pies = pies, 
                CurrentCategory = currentCategory
            });

        }// end PieController ()


        // Adding the view for ./Views/Pie/Details.cshtml
        public IActionResult Details (int id)
        {
            var pie = _pieRepository.GetPieById(id);

            if (pie == null)
                return NotFound();

            return View(pie); 

        }// end Details()

    }// end class 
}
