using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using PieShop.Models;
using PieShop.ViewModels;

namespace PieShop.Components
{
    public class ShoppingCartSummary: ViewComponent
    {
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartSummary (ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart; 
        }

        public IViewComponentResult Invoke ()
        {
            /*
            // Dummy data for shopping cart
            var items = new List<ShoppingCartItem>()
            {
                new ShoppingCartItem(),
                new ShoppingCartItem() 
                
            };
            */

            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart, 
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };


            return View(shoppingCartViewModel); 
        }// end Invoke()

    }// end class
}
