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
    public class TagListController : Controller
    {
        private ApplicationDbContext _db;
        public TagListController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var data = _db.tagLists.ToList();
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
        public async Task<ActionResult> Create(TagLists tagLists)
        {
            if (ModelState.IsValid)
            {
                _db.tagLists.Add(tagLists);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // Edit Action Method
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tagLists = await _db.tagLists.FindAsync(id);
            if (tagLists == null)
            {
                return NotFound();
            }
            return View(tagLists);
        }

        //Edit Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TagLists tagLists)
        {
            if (ModelState.IsValid)
            {
                _db.Update(tagLists);
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
            var tagLists = await _db.tagLists.FindAsync(id);
            if (tagLists == null)
            {
                return NotFound();
            }
            return View(tagLists);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tagLists = await _db.tagLists.FindAsync(id);
            if (tagLists == null)
            {
                return NotFound();
            }
            return View(tagLists);
        }

        //Delete Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, TagLists tagLists)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != tagLists.Id)
            {
                return NotFound();
            }
            var tagList = _db.tagLists.Find(id);
            if (tagList == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(tagList);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
