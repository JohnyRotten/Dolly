using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dolly.Models
{
    public sealed class Item
    {
        [Key] public int ItemId { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Required] public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>(); 
        public List<Comment> Comments { get; set; } = new List<Comment>(); 
        public List<Order> Orders { get; set; }

        [NotMapped]
        public int? FirstPhotoId => Photos.FirstOrDefault()?.PhotoId;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); 
    }
}