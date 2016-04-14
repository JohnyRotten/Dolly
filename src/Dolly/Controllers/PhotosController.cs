using System.Threading.Tasks;
using Dolly.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace Dolly.Controllers
{
    public class PhotosController : BaseController
    {

        private readonly ApplicationDbContext _db;

        public PhotosController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Show(int? id)
        {
            if (id != null)
            {
                var photo = await _db.Photos.SingleAsync(p => p.PhotoId == id);
                if (photo != null)
                {
                    return File(photo.Source, "image/jpeg");
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var photo = await _db.Photos.Include(p => p.Item).SingleAsync(p => p.PhotoId == id);
                if (photo != null)
                {
                    var item = photo.Item;
                    _db.Entry(photo).State = EntityState.Deleted;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Edit", "Items", new {Id = item.ItemId});
                }
            }
            return HttpNotFound();
        }

    }
}