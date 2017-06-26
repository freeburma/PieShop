using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieShop.Models
{
    public class MockCategoryRepository : ICategoryRepository
    {
        /*
            This is a hardcoded data. 
         */
        public IEnumerable<Category> Categories
        {
            get
            {
                return new List<Category>
                {
                    new Category{CategoryId=1, CategoryName="Fruit Pies", Descirption="All-fruity pies"},
                    new Category{CategoryId=2, CategoryName="Cheese Cakes", Descirption="Cheesey all the way"},
                    new Category{CategoryId=3, CategoryName="Seasonal Pies", Descirption="Get in the mood for a seasonal pie"},

                };
            }
        }// end Categories()

        
    }
}
