using System;
using Dolly.Models;
using Microsoft.AspNet.Http;
using System.Linq;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;

namespace Dolly.Services
{
    public class CartProvider : ICartProvider
    {

        private readonly ApplicationDbContext _db;
        private readonly HttpContext _context;
        private readonly ILogger<CartProvider> _logger; 
        private const string CartId = "CartId";

        public CartProvider(ApplicationDbContext db, 
                            IHttpContextAccessor accessor,
                            ILogger<CartProvider> logger)
        {
            _db = db;
            _context = accessor.HttpContext;
            _logger = logger;
        }
        
        public Cart Cart
        {
            get
            {
                var guid = _context.Request.Cookies[CartId];
                Guid id;
                Cart cart = null;
                if (Guid.TryParse(guid, out id))
                {
                    cart = _db.Carts.Include(i => i.Items).FirstOrDefault(c => c.CartId == id);
                }
                return cart ?? CreateCart();
            }
        }

        private Cart CreateCart()
        {
            var cart = new Cart();
            _db.Carts.Add(cart);
            _db.SaveChanges();
            _context.Response.Cookies.Append(CartId, cart.CartId.ToString());
            return cart;
        }

        public bool AddItem(Item item)
        {
            try
            {
                var cart = Cart;
                if (cart == null) throw new Exception("Cart not found");
                cart.Items.Add(item);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}\r\n{e.StackTrace}");
                return false;
            }
            return true;
        }

        public bool RemoveItem(Item item)
        {
            try
            {
                var cart = Cart;
                if (cart == null) throw new Exception("Cart not found");
                cart.Items.Remove(item);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}\r\n{e.StackTrace}");
                return false;
            }
            return true;
        }
    }
}