using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

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
            return View(_db.ProductTypes.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["Create"] = "Product Type has been saved.";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(productTypes);
        }

        [HttpGet]
        public IActionResult Edit(int ? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(Id);
            if(productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                TempData["Update"] = "Product Type update successfully.";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(productTypes);
        }

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(Id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes productTypes)
        {
             return RedirectToAction(actionName: nameof(Index));
            
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(Id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int?id, ProductTypes productTypes)
        {
            if(id == null)
            {
                return NotFound();
            }
            if(id != productTypes.Id)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                TempData["Delete"] = "Product Type delete successfully.";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(productType);
        }
    }
}