using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Dolly.Models;
using Microsoft.AspNet.Authorization;

namespace Dolly.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;    
        }
        
        [Authorize]
        public async Task<IActionResult> Index() => View(await _db.Categories.Include(c => c.Parent).ToListAsync());
        
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await CategoriesList();
            return View();
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await CategoriesList();
            return View(category);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var category = await _db.Categories.SingleAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = await CategoriesList();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Update(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await CategoriesList();
            return View(category);
        }
        
        [ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var category = await _db.Categories.SingleAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.Categories.SingleAsync(m => m.CategoryId == id);
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task<SelectList> CategoriesList() => new SelectList(await _db.Categories.ToListAsync(), "CategoryId", "Title");

    }
}
