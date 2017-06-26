using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PieShop.Models;
using PieShop.ViewModels;

/*
    Creating Web API 
 */

namespace PieShop.Controllers
{
    // Adding the API related route -- static part
    [Route("api/[controller]")]
    public class PieDataController : Controller
    {
        private readonly IPieRepository _pieRepository;

        /*
            Loading more piles on user request. Dynamically created list
         */ 
        public PieDataController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        // Addring for HTTP GET request from browser 
        [HttpGet]
        public IEnumerable<PieViewModel> LoadMorePies ()
        {
            IEnumerable<Pie> dbPies = null;

            dbPies = _pieRepository.Pies.OrderBy(p => p.PieId).Take(10);

            List<PieViewModel> pies = new List<PieViewModel>();

            foreach (var dbPie in dbPies)
            {
                pies.Add(MapDbPieToPieViewModel(dbPie)); 
            }

            return pies;

        }// end LoadMorePies ()

        /*
            Adding from Database to PieViewModel. 
            
         */
        private PieViewModel MapDbPieToPieViewModel (Pie dbPie)
        {
            return new PieViewModel()
            {
                PieId = dbPie.PieId,
                Name = dbPie.Name,
                Price = dbPie.Price,
                ShortDescription = dbPie.ShortDescription,
                ImageThumbnailUrl = dbPie.ImageThumbnailUrl
            };

        }// end MapDbPieToPieViewModel()

    }
}
