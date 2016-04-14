using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dolly.Models
{
    public class OrderItem
    {
        [Key] public int OrderItemId { get; set; }
        [Required] public int ItemId { get; set; } 
        public Item Item { get; set; }
        [Required] public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Count { get; set; } = 0;
    }
}