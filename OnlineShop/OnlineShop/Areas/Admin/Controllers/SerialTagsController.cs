using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SerialTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SerialTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SerialTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.SerialTags.ToListAsync());
        }

        // GET: Admin/SerialTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serialTags = await _context.SerialTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serialTags == null)
            {
                return NotFound();
            }

            return View(serialTags);
        }

        // GET: Admin/SerialTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SerialTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SerialTag")] SerialTags serialTags)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serialTags);
                await _context.SaveChangesAsync();
                TempData["Create"] = "Serial Tag has been saved.";
                return RedirectToAction(nameof(Index));
            }
            return View(serialTags);
        }

        // GET: Admin/SerialTags/Edit/5
        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var serialTags = _context.SerialTags.Find(Id);
            if (serialTags == null)
            {
                return NotFound();
            }
            return View(serialTags);
        }

        // POST: Admin/SerialTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SerialTags serialTags)
        {
            if (ModelState.IsValid)
            {
                _context.Update(serialTags);
                await _context.SaveChangesAsync();
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(serialTags);
        }
        
        // GET: Admin/SerialTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serialTags = await _context.SerialTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serialTags == null)
            {
                return NotFound();
            }

            return View(serialTags);
        }

        // POST: Admin/SerialTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serialTags = await _context.SerialTags.FindAsync(id);
            _context.SerialTags.Remove(serialTags);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SerialTagsExists(int id)
        {
            return _context.SerialTags.Any(e => e.Id == id);
        }
    }
}
