using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace PieShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext; 

        private ShoppingCart (AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; 
        }

        // ShoppingCart is identify by ShoppingCartId()
        public string ShoppingCartId { get; set; }

        // Shopping Cart items list
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        // Getting the shopping cart instance
        public static ShoppingCart GetCart (IServiceProvider services)
        {
            // Creating a session
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            // Dependency injection to get DB
            var context = services.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };

        }// end ShoppingCart ()

        /*
            
         */
        public void AddToCart (Pie pie, int amount)
        {
            var shoppingCartItem = _appDbContext.ShoppingCartItems.SingleOrDefault(
                    s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId
                );

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };

                _appDbContext.ShoppingCartItems.Add(shoppingCartItem); 
            }
            else
            {
                shoppingCartItem.Amount++; 
            }

            _appDbContext.SaveChanges();

        }// end AddToCart ()

        public int RemoveFromCart (Pie pie)
        {
            var shoppingCartItem = _appDbContext.ShoppingCartItems.SingleOrDefault(
                    s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId
                );

            var localAmount = 0; 

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount; 
                }
                else
                {
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }// end if

            _appDbContext.SaveChanges();

            return localAmount; 

        }// end RemoveFromCart ()

        public List<ShoppingCartItem> GetShoppingCartItems ()
        {
            return ShoppingCartItems ?? (ShoppingCartItems =
                    _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                    .Include(s => s.Pie)
                    .ToList()
                );

        }// end GetShoppingCartItems ()

        public void ClearCart()
        {
            var cartItems = _appDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal ()
        {
            var total = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Pie.Price * c.Amount).Sum();

            return total;

        }// GetShoppingCartTotal ()


    }// end class
}
