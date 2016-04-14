using System.Threading.Tasks;
using Dolly.Models;
using Dolly.Services;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace Dolly.Controllers
{
    public class CartController : BaseController
    {

        private readonly ICartProvider _provider;

        private readonly ApplicationDbContext _db;

        public CartController(ICartProvider provider, ApplicationDbContext db)
        {
            _provider = provider;
            _db = db;
        }

        public IActionResult Index() => View(_provider.Cart);
        
        [HttpPost]
        public async Task<IActionResult> Add(int id, string returnUri)
        {
            var item = await _db.Items.SingleAsync(i => i.ItemId == id);
            if (item != null)
            {
                _provider.AddItem(item);
                return LocalRedirect(returnUri);
            }
            return HttpBadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id, string returnUri)
        {
            var item = await _db.Items.SingleAsync(i => i.ItemId == id);
            if (item != null)
            {
                _provider.RemoveItem(item);
                return LocalRedirect(returnUri);
            }
            return HttpBadRequest();
        }

    }
}