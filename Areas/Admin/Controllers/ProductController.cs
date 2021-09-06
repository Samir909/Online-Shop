using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
          
        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        } 
        public IActionResult Index()
        {
            var data = _db.products.Include(c => c.ProductTypes).Include(f => f.TagLists).ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Index(decimal ? lowAmount, decimal ? largeAmount)
        {
            var product =  _db.products.Include(c => c.ProductTypes).Include(c => c.TagLists).Where(c => c.Price >= lowAmount && c.Price < largeAmount).ToList();
            if (lowAmount == null && largeAmount == null)
            {
                product = _db.products.Include(c => c.ProductTypes).Include(c => c.TagLists).ToList();
            }
            return View(product);
        }
        //Create Get Action Method
        public ActionResult Create()
        {
            ViewData["ProductTypeID"] = new SelectList(_db.productTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.tagLists.ToList(), "Id", "TagList");
            return View();
        }


        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Products products,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = await _db.products.FirstOrDefaultAsync(c => c.Name == products.Name);
                if (searchProduct!=null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["ProductTypeID"] = new SelectList(_db.productTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.tagLists.ToList(), "Id", "TagList");
                    return View(products);
                }
                if(image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "/Images" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Images/noImage.png";
                }
                _db.products.Add(products);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been save successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // Edit Action Method
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            ViewData["ProductTypeID"] = new SelectList(_db.productTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.tagLists.ToList(), "Id", "TagList");
            var product = await _db.products.Include(c => c.ProductTypes).Include(c => c.TagLists).FirstOrDefaultAsync(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Edit Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "/Images" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Images/noImage.png";
                }
                _db.products.Update(products);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been save successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //Get Details Action Method
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _db.products.Include(c => c.ProductTypes).Include(c => c.TagLists).FirstOrDefaultAsync(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _db.products.Include(c => c.ProductTypes).Include(c => c.TagLists).FirstOrDefaultAsync(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfarmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _db.products.FirstOrDefaultAsync(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _db.products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
