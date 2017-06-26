using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PieShop.Models;

namespace PieShop.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDpContext;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(AppDbContext appDpContext, ShoppingCart shoppingCart)
        {
            _appDpContext = appDpContext;
            _shoppingCart = shoppingCart; 

        }// end constructor


        public void CreateOrder (Order order)
        {
            order.OrderPlaced = DateTime.Now;

            _appDpContext.Orders.Add(order);

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            // Looping all the shopping cart items 
            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Amount,
                    PieId = shoppingCartItem.Pie.PieId,
                    OrderId = order.OrderId, 
                    Price = shoppingCartItem.Pie.Price

                };


                // Add to ...
                _appDpContext.OrderDetails.Add(orderDetail); 
            }

            // Adding to database
            _appDpContext.SaveChanges(); 


        }// end CreateOrder ()

    }// end class 
}
