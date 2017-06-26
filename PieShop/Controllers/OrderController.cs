using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PieShop.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PieShop.Controllers
{
    public class OrderController : Controller
    {

        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;


        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart; 

        }// end constructor 

        [Authorize] // Only login user can use 
        public IActionResult Checkout ()
        {
            return View();
        }

        // Creating a view for order validation occurs only HTTP Post method call
        // Model binding will capture all the events and create an order instance
        [HttpPost]
        [Authorize] // Only login user can use
        public IActionResult Checkout (Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items; 

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some pies first"); 

            }

            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();

                return RedirectToAction("CheckoutComplete"); 
            }

            return View(order);  // Same view return if errors found
        }

        // Adding the check compleste action view 
        public IActionResult CheckoutComplete ()
        {
            ViewBag.CheckoutCompleteMessage = "Thank you for order. You'll soon enjoy our delicious pies!";

            return View(); 
        }

    }// end class 

    
}
