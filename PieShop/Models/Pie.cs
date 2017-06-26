using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieShop.Models     // Added 20-Jun-17 11:58 am
{
    public class Pie
    {
        public int PieId { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string AllergyInformation { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string ImageThumbnailUrl { get; set; }

        public bool IsPieOfTheWeek { get; set; }

        public bool InStock { get; set; }

        public int CategoryId { get; set; }

        /*
            This will virtual key word will work with EntityFrameWork
            Lazy loading : not working frequently (Virtual): one time loading
         */
        public virtual Category Category { get; set; }
    }
}
