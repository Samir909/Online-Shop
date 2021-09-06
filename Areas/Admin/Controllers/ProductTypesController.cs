using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var data = _db.productTypes.ToList();
            return View(data);
        }

        //Create Get Action Method
        public ActionResult Create()
        {
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.productTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type has been save successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // Edit Action Method
        public async Task<ActionResult> Edit(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTypes = await _db.productTypes.FindAsync(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        //Edit Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // Get Details Action Method
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTypes = await _db.productTypes.FindAsync(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        ////Details Post Action Method
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public  IActionResult Details(ProductTypes productTypes)
        //{

        //    return View(nameof(Index));
        //}

        //Delete Post Action Method
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTypes = await _db.productTypes.FindAsync(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        //Delete Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id,ProductTypes productTypes)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != productTypes.Id)
            {
                return NotFound();
            }
            var productType = _db.productTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
