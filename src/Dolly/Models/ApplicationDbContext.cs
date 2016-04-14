using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System.Linq;
using Microsoft.Data.Entity.Internal;

namespace Dolly.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Photo> Photos { get; set; }   
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }  
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; } 
        public DbSet<OrderItem> OrderItems { get; set; } 

        public async Task<List<Comment>> GetCommentByType<T>(int? id = null)
        {
            var type = typeof (T).DisplayName(false);
            if (id == null)
            {
                return (await Comments.ToListAsync()).Where(c => c.ItemType == type).ToList();
            }
            return (await Comments.ToListAsync()).Where(c => c.ItemType == type && c.ObjectId == id).ToList();
        }
    }
}
