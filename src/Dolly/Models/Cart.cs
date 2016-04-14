using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dolly.Models
{
    public class Cart
    {
        [Key] public Guid CartId { get; set; }
        public ICollection<Item> Items { get; } = new List<Item>();
        [NotMapped] public decimal Price => Items.Sum(i => i.Price);
    }
}