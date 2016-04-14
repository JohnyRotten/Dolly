using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Dolly.Models;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using Dolly.ViewModels;
using System.Linq;
using Dolly.Services;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace Dolly.Controllers
{
    public class ItemsController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public ItemsController(ApplicationDbContext db)
        {
            _db = db;    
        }

        public async Task<IActionResult> Index(int? category)
        {
            IEnumerable<Item> items;
            if (category == null)
            {
                items = await _db.Items.Include(i => i.Photos).Include(i => i.Category).ToListAsync();
            }
            else
            {
                items = await _db.Items.Include(i => i.Photos).Include(i => i.Category).Where(i => i.CategoryId == (int)category).ToListAsync();
            }
            return View(items);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var item = await Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(new ItemViewModel
            {
                Item = item,
                Comments = await _db.GetCommentByType<Item>(id)
            });
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await CategoriesList();
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            item.Category = await _db.Categories.SingleAsync(c => c.CategoryId == item.CategoryId);
            if (ModelState.IsValid)
            {
                _db.Items.Add(item);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await CategoriesList();
            return View(item);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var item = await Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = await CategoriesList();
            return View(item);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                _db.Update(item);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await CategoriesList();
            return View(item);
        }

        [ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var item = await _db.Items.SingleAsync(m => m.ItemId == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = new Item { ItemId = id };
            _db.Entry(item).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Comments(Comment comment)
        {
            try
            {
                var uid = User.GetUserId();
                comment.User = await _db.Users.FirstOrDefaultAsync(u => u.Id == uid);
                comment.ItemType = nameof (Item);
                _db.Comments.Add(comment);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { Id = comment.ObjectId });
            }
            catch
            {
                //
            }
            return HttpBadRequest();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files, int? id)
        {
            if (id != null)
            {
                var item = await Find(id);
                if (item != null)
                {
                    foreach (var file in files.Where(file => file.Length > 0))
                    {
                        try
                        {
                            using (var stream = file.OpenReadStream())
                            {
                                var bytes = new byte[stream.Length];
                                await stream.ReadAsync(bytes, 0, bytes.Length);
                                _db.Photos.Add(new Photo
                                {
                                    Item = item,
                                    Source = bytes
                                });
                            }
                        }
                        catch
                        {
                            //
                        }
                    }
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Edit), new {Id = item.ItemId});
                }
            }
            return HttpNotFound();
        }

        private async Task<Item> Find(int? id)
        {
            if (id != null)
            {
                return await _db.Items.Include(i => i.Photos).SingleAsync(i => i.ItemId == id);
            }
            return null;
        }

        private async Task<SelectList> CategoriesList() => new SelectList(await _db.Categories.ToListAsync(), "CategoryId", "Title");

    }
}
