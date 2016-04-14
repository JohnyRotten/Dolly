using System.Threading.Tasks;
using Dolly.Models;
using Dolly.ViewModels;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Authorization;
using System.Linq;
using System.Security.Claims;

namespace Dolly.Controllers
{
    public class PostsController : BaseController
    {

        private readonly ApplicationDbContext _db;

        public PostsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index() => View(await _db.Posts.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var post = await _db.Posts.FirstOrDefaultAsync(p => p.PostId == id);
                if (post != null)
                {
                    return View(new PostViewModel
                    {
                        Post = post,
                        Comments = await _db.GetCommentByType<Post>(id)
                    });
                }
            }
            return HttpNotFound();
        }

        [HttpGet]
        public IActionResult New() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> New(Post post)
        {
            var uid = User.GetUserId();
            post.User = await _db.Users.FirstOrDefaultAsync(u => u.Id == uid);
            if (ModelState.IsValid)
            {
                _db.Posts.Add(post);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { Id = post.PostId });
            }
            return View(post);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var post = await _db.Posts.FirstOrDefaultAsync(p => p.PostId == id);
                if (post != null)
                {
                    return View(post);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(post).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {Id = post.PostId});
            }
            return View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var post = new Post {PostId = (int)id};
                _db.Entry(post).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Comments(Comment comment)
        {
            try
            {
                var uid = User.GetUserId();
                comment.User = await _db.Users.FirstOrDefaultAsync(u => u.Id == uid);
                comment.ItemType = nameof (Post);
                _db.Comments.Add(comment);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {Id = comment.ObjectId});
            }
            catch
            {
                //
            }
            return HttpBadRequest();
        }

    }
}
