using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dolly.Models
{
    public sealed class Category
    {
        [Key] public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public Category Parent { get; set; }
        [Required] public string Title { get; set; }

        [ForeignKey(nameof(ParentId))]
        public List<Category> Categories { get; set; } = new List<Category>(); 
    }
}