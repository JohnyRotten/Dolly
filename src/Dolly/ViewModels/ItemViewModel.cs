using System.Collections.Generic;
using Dolly.Models;

namespace Dolly.ViewModels
{
    public class ItemViewModel
    {
        public Item Item { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();  
    }
}