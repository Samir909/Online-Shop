using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
       // Get CheckOut method

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Checkout(Order order)
        {
            List<Products> productAdd = HttpContext.Session.Get<List<Products>>("products");
            if (productAdd != null)
            {
                foreach (var produts in productAdd)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = produts.Id;
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNo = GetOrderNo();
            _db.orders.Add(order);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Products>());
            return View();
        }
        public string GetOrderNo()
        {
            int rowCount = _db.orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
    }
}
