using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // for IdentityDbContext<IdentityUser>
using Microsoft.AspNetCore.Identity;

namespace PieShop.Models
{
    /*
        This class will work intermediate between the code and DB. 
        This keep track of the data with database
        DbContext: comes with EntityFrameworkCore (EF)
     */
    // public class AppDbContext: DbContext     // Uncomment if you want to build with another DB system
    public class AppDbContext: IdentityDbContext<IdentityUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {

        }

        // Storing the data inside the database

        public DbSet<Category> Categories { get; set; }
        public DbSet<Pie> Pies { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

    }// end class 
}
