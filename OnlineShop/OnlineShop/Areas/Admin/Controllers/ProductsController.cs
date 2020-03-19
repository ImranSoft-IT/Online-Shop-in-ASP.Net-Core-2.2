using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _he;
        public ProductsController(ApplicationDbContext context, IHostingEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.ProductTypes).Include(p => p.SerialTags);
            return View(await applicationDbContext.ToListAsync());
        }

        // POST: Admin/Products
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var product = _context.Products.Include(p => p.ProductTypes).Include(p => p.SerialTags)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                product = _context.Products.Include(p => p.ProductTypes).Include(p => p.SerialTags).ToList();
            }
            return View(product);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.ProductTypes)
                .Include(p => p.SerialTags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypesId"] = new SelectList(_context.ProductTypes, "Id", "ProductType");
            ViewData["SerialTagsId"] = new SelectList(_context.SerialTags, "Id", "SerialTag");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Image,ProductColor,IsAvailable,ProductTypesId,SerialTagsId")] Products products, IFormFile image)
        {
            if(ModelState.IsValid)
            {
                var searchProduct = _context.Products.FirstOrDefault(c => c.Name == products.Name);
                if(searchProduct != null)
                {
                    ViewBag.Message = "This Product is already exist";
                    ViewData["ProductTypesId"] = new SelectList(_context.ProductTypes, "Id", "ProductType");
                    ViewData["SerialTagsId"] = new SelectList(_context.SerialTags, "Id", "SerialTag");
                    return View(products);
                }
                if(image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                if(image == null)
                {
                    products.Image = "Images/noImage.jpg";
                }
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypesId"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypesId);
            ViewData["SerialTagsId"] = new SelectList(_context.SerialTags, "Id", "SerialTag", products.SerialTagsId);
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["ProductTypesId"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypesId);
            ViewData["SerialTagsId"] = new SelectList(_context.SerialTags, "Id", "SerialTag", products.SerialTagsId);
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Image,ProductColor,IsAvailable,ProductTypesId,SerialTagsId")] Products products, IFormFile image)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Images/noImage.jpg";
                }
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypesId"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypesId);
            ViewData["SerialTagsId"] = new SelectList(_context.SerialTags, "Id", "SerialTag", products.SerialTagsId);
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.ProductTypes)
                .Include(p => p.SerialTags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
