using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Dolly.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.Data.Entity;
using System.Linq;
using Dolly.Services;

namespace Dolly.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly ApplicationDbContext _db;

        private readonly ICartProvider _provider;

        public OrdersController(ApplicationDbContext db, ICartProvider provider)
        {
            _db = db;
            _provider = provider;
        }

        [Authorize]
        public async Task<IActionResult> Index() => 
            View((await _db.Orders.ToListAsync()).Where(o => o.UserId == Guid.Parse(User.GetUserId())));

        [HttpGet]
        [Authorize]
        public IActionResult Create(string returnUri)
        {
            var cart = _provider.Cart;
            if (cart.Items.Any())
            {
                var order = new Order();
                foreach (var item in cart.Items)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        Count = 1,
                        Item = item
                    });
                }
                return View(order);
            }
            ViewData["Message"] = "Корзина пуста";
            return LocalRedirect(returnUri);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Confirm(Order order)
        {
            var user = await _db.Users.SingleAsync(u => u.Id == User.GetUserId());
            order.UserId = Guid.Parse(user.Id);
            if (ModelState.IsValid)
            {
                var cart = _provider.Cart;
                foreach (var item in order.OrderItems)
                {
                    var tmp = await _db.Items.FirstOrDefaultAsync(i => i.ItemId == item.ItemId);
                    if (tmp != null)
                    {
                        order.Price += tmp.Price * item.Count;
                        cart.Items.Remove(tmp);
                    }
                }
                order.Status = OrderStatus.Processed;
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create), order);
        }

    }
}
