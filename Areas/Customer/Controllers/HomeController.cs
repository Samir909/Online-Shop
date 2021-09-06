using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
      
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = _db.products.Include(c => c.ProductTypes).Include(c => c.TagLists).ToList();
            return View(data);
        }

        // Get Details Action Method
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var products = await _db.products.Include(c=>c.ProductTypes).FirstOrDefaultAsync(c=>c.Id==id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        [HttpPost]
        [ActionName("Details")]
        public async Task<ActionResult> ProductDetails(int? id)
        {
            List<Products> productAdd = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var products = await _db.products.Include(c => c.ProductTypes).FirstOrDefaultAsync(c => c.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            productAdd = HttpContext.Session.Get<List<Products>>("products");
            if (productAdd == null)
            {
                productAdd = new List<Products>();
            }
            productAdd.Add(products);
            HttpContext.Session.Set("products", productAdd);
            return View(products);
        }

        [HttpPost]
        public IActionResult Remove(int?id)
        {
            List<Products> productAdd = HttpContext.Session.Get<List<Products>>("products");
            if (productAdd != null)
            {
                var product = productAdd.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    productAdd.Remove(product);
                    HttpContext.Session.Set("products", productAdd);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [ActionName("Remove")]
        public IActionResult RemoveCartProduct(int? id)
        {
            List<Products> productAdd = HttpContext.Session.Get<List<Products>>("products");
            if (productAdd != null)
            {
                var product = productAdd.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    productAdd.Remove(product);
                    HttpContext.Session.Set("products", productAdd);
                }
            }
            return RedirectToAction(nameof(Cart));
        }
        public IActionResult Cart()
        {
            List<Products> product = HttpContext.Session.Get<List<Products>>("products");
            if (product==null)
            {
                product = new List<Products>();
            }
            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
